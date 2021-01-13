using AutoMapper;
using JetBrains.Annotations;
using Antares.Sdk;
using Lykke.Service.Assets.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using Autofac;
using Lykke.SettingsReader;
using Microsoft.Extensions.Configuration;

namespace Lykke.Service.Assets
{
    [UsedImplicitly]
    public class Startup
    {
        private readonly LykkeSwaggerOptions _swaggerOptions = new LykkeSwaggerOptions
        {
            ApiTitle = "Assets Service",
            ApiVersion = "v1"
        };

        private IReloadingManagerWithConfiguration<ApplicationSettings> _settings;
        private LykkeServiceOptions<ApplicationSettings> _lykkeOptions;


        //[UsedImplicitly]
        //public IServiceProvider ConfigureServices(IServiceCollection services)
        //{
        //    return services.BuildServiceProvider<ApplicationSettings>(options =>
        //    {
        //        options.SwaggerOptions = _swaggerOptions;

        //        options.Logs = logs =>
        //        {
        //            logs.AzureTableName = "AssetsServiceLog";
        //            logs.AzureTableConnectionStringResolver = settings => settings.AssetsService.Db.LogsConnString;
        //        };

        //        options.Extend = (sc, settingsManager) =>
        //        {
        //            sc.AddMemoryCache();
        //        };
        //    });
        //}

        [UsedImplicitly]
        public void ConfigureServices(IServiceCollection services)
        {
            (_lykkeOptions, _settings) = services.ConfigureServices<ApplicationSettings>(options =>
            {
                options.SwaggerOptions = _swaggerOptions;

                options.Logs = logs =>
                {
                    logs.AzureTableName = "AssetsServiceLog";
                    logs.AzureTableConnectionStringResolver = settings => settings.AssetsService.Db.LogsConnString;
                };

                options.Extend = (sc, settingsManager) =>
                {
                    sc.AddMemoryCache();
                };
            });
        }

        [UsedImplicitly]
        public void Configure(IApplicationBuilder app)
        {
            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });
        }

        [UsedImplicitly]
        public void ConfigureContainer(ContainerBuilder builder)
        {
            ConfigureAutoMapper();

            var configurationRoot = new ConfigurationBuilder()
                .AddEnvironmentVariables()
                .Build();

            builder.ConfigureContainerBuilder(_lykkeOptions, configurationRoot, _settings);
        }

        private void ConfigureAutoMapper()
        {
            Mapper.Initialize(cfg =>
            {
                cfg.AddProfiles(typeof(AutoMapperProfile));
                cfg.AddProfiles(typeof(Repositories.AutoMapperProfile));
                cfg.AddProfiles(typeof(Services.AutoMapperProfile));
            });

            Mapper.AssertConfigurationIsValid();
        }

    }
}
