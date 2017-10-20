﻿using System;
using System.ComponentModel.DataAnnotations;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using AutoMapper;
using Common.Log;
using JetBrains.Annotations;
using Lykke.AzureQueueIntegration;
using Lykke.Common.ApiLibrary.Middleware;
using Lykke.Common.ApiLibrary.Swagger;
using Lykke.Logs;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Repositories;
using Lykke.Service.Assets.Responses.V2;
using Lykke.Service.Assets.Services;
using Lykke.SettingsReader;
using Lykke.SlackNotification.AzureQueue;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Converters;

namespace Lykke.Service.Assets
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly IConfigurationRoot _configuration;

        private IContainer _applicationContainer;


        public Startup(IHostingEnvironment env)
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(typeof(AutoMapperProfile));
                cfg.AddProfiles(typeof(Repositories.AutoMapperProfile));
                cfg.AddProfiles(typeof(Services.AutoMapperProfile));
            });

            Mapper.AssertConfigurationIsValid();

            var builder = new ConfigurationBuilder()
                .SetBasePath(env.ContentRootPath)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
                .AddEnvironmentVariables();


            _configuration = builder.Build();
        }


        public IServiceProvider ConfigureServices(IServiceCollection services)
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
            
            var settings = _configuration.LoadSettings<ApplicationSettings>("ConnectionStrings:SettingsUrl");
            var log      = CreateLog(services, settings.CurrentValue);
            var builder  = new ContainerBuilder();

            builder.RegisterModule(new ApiModule(settings, log));
            builder.RegisterModule(new RepositoriesModule(log, settings));
            builder.RegisterModule(new ServicesModule());

            builder.Populate(services);

            _applicationContainer = builder.Build();

            return new AutofacServiceProvider(_applicationContainer);
        }
        
        public void Configure(IApplicationBuilder app, IHostingEnvironment env, ILoggerFactory loggerFactory, IApplicationLifetime appLifetime)
        {
            loggerFactory.AddConsole(_configuration.GetSection("Logging"));
            loggerFactory.AddDebug();

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

                return Error.Create(errorMessage);
            });

            app.UseMvc();
            app.UseSwagger();
            app.UseSwaggerUi();
            app.UseStaticFiles();

            appLifetime.ApplicationStopped.Register(CleanUp);
        }

        private void CleanUp()
        {
            _applicationContainer.Dispose();
        }

        private static ILog CreateLog(IServiceCollection services, ApplicationSettings settings)
        {
            var consoleLogger   = new LogToConsole();
            var aggregateLogger = new AggregateLogger();

            aggregateLogger.AddLog(consoleLogger);

            // Creating slack notification service, which logs own azure queue processing messages to aggregate log
            var slackService = services.UseSlackNotificationsSenderViaAzureQueue(new AzureQueueSettings
            {
                ConnectionString = settings.SlackNotifications.AzureQueue.ConnectionString,
                QueueName        = settings.SlackNotifications.AzureQueue.QueueName
            }, aggregateLogger);

            var dbLogConnectionString = settings.AssetsService.Logs.DbConnectionString;

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

                //var persistenceManager = new LykkeLogToAzureStoragePersistenceManager
                //(
                //    appName,
                //    AzureTableStorage<LogEntity>.Create(() => dbLogConnectionString, "AssetsServiceLog", consoleLogger),
                //    consoleLogger
                //);



                //var azureStorageLogger = new LykkeLogToAzureStorage
                //(
                //    appName,
                //    persistenceManager,
                //    slackNotificationsManager,
                //    consoleLogger
                //);

                //azureStorageLogger.Start();

                //aggregateLogger.AddLog(azureStorageLogger);
            }

            return aggregateLogger;
        }
    }
}