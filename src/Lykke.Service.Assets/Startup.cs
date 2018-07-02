using System;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.ApiLibrary.Middleware;
using Lykke.Common.ApiLibrary.Swagger;
using Lykke.Common.Log;
using Lykke.Logs;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Modules;
using Lykke.Service.Assets.Repositories;
using Lykke.Service.Assets.Responses.V2;
using Lykke.Service.Assets.Services;
using Lykke.SettingsReader;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json.Converters;
using LogLevel = Microsoft.Extensions.Logging.LogLevel;

namespace Lykke.Service.Assets
{
    [UsedImplicitly]
    public class Startup
    {
        private IConfigurationRoot  Configuration { get; set; }
        private IContainer          ApplicationContainer { get; set; }
        private ILog                Log { get; set; }
        private IHealthNotifier     HealthNotifier { get; set; }


        public Startup(IHostingEnvironment env)
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddEnvironmentVariables();

            Configuration = builder.Build();
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
                Log?.Critical(e);

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

                var settings = Configuration.LoadSettings<ApplicationSettings>();
                var assetServiceSettings = settings.Nested(x => x.AssetsService);

                services.AddDistributedRedisCache(options =>
                {
                    options.Configuration = settings.CurrentValue.AssetsService.RadisSettings.RedisConfiguration;
                    options.InstanceName = settings.CurrentValue.AssetsService.RadisSettings.InstanceName;
                });

                services.AddLykkeLogging(
                    assetServiceSettings.ConnectionString(x => x.Logs.DbConnectionString),
                    "AssetsServiceLog",
                    settings.CurrentValue.SlackNotifications.AzureQueue.ConnectionString,
                    settings.CurrentValue.SlackNotifications.AzureQueue.QueueName,
                    logging =>
                    {
                        // Just for example:

                        logging.ConfigureAzureTable = options =>
                        {
                            options.BatchSizeThreshold = 1000;
                            options.MaxBatchLifetime = TimeSpan.FromSeconds(10);
                        };

                        logging.ConfigureConsole = options =>
                        {
                            options.IncludeScopes = true;
                        };

                        logging.ConfigureEssentialSlackChannels = options =>
                        {
                            options.SpamGuard.SetMutePeriod(LogLevel.Error, TimeSpan.FromMinutes(5));
                        };
                    });

                var builder = new ContainerBuilder();
                builder.Populate(services);

                builder.RegisterModule(new ApiModule(settings));
                builder.RegisterModule(new CqrsModule(settings.Nested(x => x.AssetsService)));
                builder.RegisterModule(new RepositoriesModule(settings));
                builder.RegisterModule(new ServicesModule());
                
                ApplicationContainer = builder.Build();

                Log = ApplicationContainer.Resolve<ILogFactory>().CreateLog(this);

                HealthNotifier = ApplicationContainer.Resolve<IHealthNotifier>();

                return new AutofacServiceProvider(ApplicationContainer);
            }
            catch (Exception e)
            {
                Log?.Critical(e);

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

                app.UseLykkeMiddleware(ex =>
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
                app.UseSwagger(c =>
                {
                    c.PreSerializeFilters.Add((swagger, httpReq) => swagger.Host = httpReq.Host.Value);
                });
                app.UseSwaggerUI(x =>
                {
                    x.RoutePrefix = "swagger/ui";
                    x.SwaggerEndpoint("/swagger/v1/swagger.json", "v1");
                });
                app.UseStaticFiles();
                
                appLifetime.ApplicationStarted.Register(()  => StartApplication().Wait());
                appLifetime.ApplicationStopping.Register(() => StopApplication().Wait());
                appLifetime.ApplicationStopped.Register(CleanUp);
            }
            catch (Exception e)
            {
                Log?.Critical(e);

                throw;
            }
        }

        private async Task StartApplication()
        {
            try
            {
                // NOTE: Service not yet receive and process requests here

                await ApplicationContainer.Resolve<IStartupManager>().StartAsync();

                HealthNotifier.Notify("Started");
            }
            catch (Exception ex)
            {
                Log.Critical(ex);
                throw;
            }
        }

        private async Task StopApplication()
        {
            try
            {
                // NOTE: Service still can receive and process requests here, so take care about it if you add logic here.

                await ApplicationContainer.Resolve<IShutdownManager>().StopAsync();
            }
            catch (Exception ex)
            {
                Log?.Critical(ex);
                throw;
            }
        }

        private void CleanUp()
        {
            try
            {
                // NOTE: Service can't receive and process requests here, so you can destroy all resources

                HealthNotifier?.Notify("Terminating");

                ApplicationContainer.Dispose();
            }
            catch (Exception ex)
            {
                Log?.Critical(ex);

                (Log as IDisposable)?.Dispose();
                throw;
            }
        }
    }
}
