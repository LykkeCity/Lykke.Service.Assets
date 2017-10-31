using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using AzureStorage.Tables;
using Common.Log;
using JetBrains.Annotations;
using Lykke.AzureQueueIntegration;
using Lykke.Common.ApiLibrary.Middleware;
using Lykke.Common.ApiLibrary.Swagger;
using Lykke.Logs;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories;
using Lykke.Service.Assets.Responses.V2;
using Lykke.Service.Assets.Services;
using Lykke.SettingsReader;
using Lykke.SlackNotification.AzureQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.Assets
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly IConfigurationRoot  _configuration;
        private readonly IHostingEnvironment _environment;

        private IContainer _applicationContainer;
        private ILog       _log;


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            _configuration = builder.Build();
            _environment   = env;

            ConfigureAutoMapper();
        }


        private void ConfigureAutoMapper()
        {
            try
            {
                Mapper.Initialize(cfg =>
                {
                    cfg.AddProfiles(typeof(AutoMapperProfile));
                    cfg.AddProfiles(typeof(Repositories.AutoMapperProfile));
                    cfg.AddProfiles(typeof(Services.AutoMapperProfile));
                });

                Mapper.AssertConfigurationIsValid();
            }
            catch (Exception e)
            {
                _log?.WriteFatalErrorAsync(nameof(Startup), nameof(ConfigureAutoMapper), "", e).Wait();

                throw;
            }
        }

        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            try
            {
                services
                    .AddMvc()
                    .AddJsonOptions(options =>
                    {
                        options.SerializerSettings.Converters.Add(new StringEnumConverter());
                        options.SerializerSettings.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                    });

                services
                    .AddSwaggerGen(options =>
                    {
                        options.DefaultLykkeConfiguration("v1", "Assets Service");
                    });

                services.AddMemoryCache();

                var settings = _configuration.LoadSettings<ApplicationSettings>();

                _log = CreateLogWithSlack(services, settings);

                var builder = new ContainerBuilder();

                builder.RegisterInstance(_log)
                    .As<ILog>()
                    .SingleInstance();

                builder.RegisterModule(new ApiModule(settings, _log));
                builder.RegisterModule(new RepositoriesModule(settings, _log));
                builder.RegisterModule(new ServicesModule());

                builder.Populate(services);

                _applicationContainer = builder.Build();

                return new AutofacServiceProvider(_applicationContainer);
            }
            catch (Exception e)
            {
                _log?.WriteFatalErrorAsync(nameof(Startup), nameof(ConfigureServices), "", e).Wait();

                throw;
            }
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, IApplicationLifetime appLifetime)
        {
            ConfigureAutoMapper();

            try
            {
                if (env.IsDevelopment())
                {
                    app.UseDeveloperExceptionPage();
                }

                app.UseLykkeMiddleware(Constants.ComponentName, ex =>
                {
                    string errorMessage;

                    switch (ex)
                    {
                        case InvalidOperationException ioe:
                            errorMessage = $"Invalid operation: {ioe.Message}";
                            break;
                        case ValidationException ve:
                            errorMessage = $"Validation error: {ve.Message}";
                            break;
                        default:
                            errorMessage = "Technical problem";
                            break;
                    }

                    return Error.Create(Constants.ComponentName, errorMessage);
                });

                app.UseMvc();
                app.UseSwagger();
                app.UseSwaggerUi();
                app.UseStaticFiles();
                
                appLifetime.ApplicationStarted.Register(()  => StartApplication().Wait());
                appLifetime.ApplicationStopping.Register(() => StopApplication().Wait());
                appLifetime.ApplicationStopped.Register(()  => CleanUp().Wait());
            }
            catch (Exception e)
            {
                _log?.WriteFatalErrorAsync(nameof(Startup), nameof(Configure), "", e).Wait();

                throw;
            }
        }

        private async Task StartApplication()
        {
            try
            {
                // NOTE: Service not yet recieve and process requests here

                await _applicationContainer.Resolve<IStartupManager>().StartAsync();

                await _log.WriteMonitorAsync("", "", "Started");
            }
            catch (Exception ex)
            {
                await _log.WriteFatalErrorAsync(nameof(Startup), nameof(StartApplication), "", ex);
                throw;
            }
        }

        private async Task StopApplication()
        {
            try
            {
                // NOTE: Service still can recieve and process requests here, so take care about it if you add logic here.

                await _applicationContainer.Resolve<IShutdownManager>().StopAsync();
            }
            catch (Exception ex)
            {
                if (_log != null)
                {
                    await _log.WriteFatalErrorAsync(nameof(Startup), nameof(StopApplication), "", ex);
                }
                throw;
            }
        }

        private async Task CleanUp()
        {
            try
            {
                // NOTE: Service can't recieve and process requests here, so you can destroy all resources

                if (_log != null)
                {
                    await _log.WriteMonitorAsync("", "", "Terminating");
                }

                _applicationContainer.Dispose();
            }
            catch (Exception ex)
            {
                if (_log != null)
                {
                    await _log.WriteFatalErrorAsync(nameof(Startup), nameof(CleanUp), "", ex);

                    (_log as IDisposable)?.Dispose();
                }
                throw;
            }
        }

        private static ILog CreateLogWithSlack(IServiceCollection services, IReloadingManager<ApplicationSettings> settings)
        {
            var consoleLogger   = new LogToConsole();
            var aggregateLogger = new AggregateLogger();

            aggregateLogger.AddLog(consoleLogger);

            // Creating slack notification service, which logs own azure queue processing messages to aggregate log
            var slackService = services.UseSlackNotificationsSenderViaAzureQueue(new AzureQueueSettings
            {
                ConnectionString = settings.CurrentValue.SlackNotifications.AzureQueue.ConnectionString,
                QueueName        = settings.CurrentValue.SlackNotifications.AzureQueue.QueueName
            }, aggregateLogger);

            var dbLogConnectionStringManager = settings.Nested(x => x.AssetsService.Logs.DbConnectionString);
            var dbLogConnectionString        = dbLogConnectionStringManager.CurrentValue;

            // Creating azure storage logger, which logs own messages to concole log
            if (!string.IsNullOrEmpty(dbLogConnectionString) && !(dbLogConnectionString.StartsWith("${") && dbLogConnectionString.EndsWith("}")))
            {
                const string appName = "Lykke.Service.Assets";

                var slackNotificationsManager = new LykkeLogToAzureSlackNotificationsManager
                (
                    appName,
                    slackService,
                    consoleLogger
                );
                
                var persistenceManager = new LykkeLogToAzureStoragePersistenceManager
                (
                    appName,
                    AzureTableStorage<LogEntity>.Create(settings.ConnectionString(x => x.AssetsService.Logs.DbConnectionString), "AssetsServiceLog", consoleLogger),
                    consoleLogger
                );



                var azureStorageLogger = new LykkeLogToAzureStorage
                (
                    appName,
                    persistenceManager,
                    slackNotificationsManager,
                    consoleLogger
                );

                azureStorageLogger.Start();

                aggregateLogger.AddLog(azureStorageLogger);
            }

            return aggregateLogger;
        }
    }
}
