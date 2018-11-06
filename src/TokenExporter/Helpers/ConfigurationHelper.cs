using System;
using System.Collections.Generic;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.Logs;
using Lykke.Logs.Loggers.LykkeConsole;
using Lykke.Service.Assets.Client;
using Lykke.SettingsReader;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace TokenExporter.Helpers
{
    public class ConfigurationHelper : IConfigurationHelper
    {
        public (IContainer resolver, ILog logToConsole) GetResolver(string assetServiceUrl)
        {
            ContainerBuilder containerBuilder = new ContainerBuilder();
            IServiceCollection collection = new Microsoft.Extensions.DependencyInjection.ServiceCollection();
            var consoleLogger = LogFactory.Create();
            consoleLogger.AddConsole(options => { options.IncludeScopes = true; });
            var log = consoleLogger.CreateLog(this);
            collection.AddSingleton<ILog>(log);
            containerBuilder.RegisterInstance(new MemoryCache(new MemoryCacheOptions()))
                .As<IMemoryCache>()
                .SingleInstance();
            containerBuilder.RegisterAssetsClient(
                new AssetServiceSettings()
                {
                    ServiceUrl = assetServiceUrl
                }, true);
            containerBuilder.Populate(collection);

            var resolver = containerBuilder.Build();
            return (resolver, log);
        }
    }
}
