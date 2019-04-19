using AutoMapper;
using JetBrains.Annotations;
using Lykke.Sdk;
using Lykke.Service.Assets.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;

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


        [UsedImplicitly]
        public IServiceProvider ConfigureServices(IServiceCollection services)
        {
            return services.BuildServiceProvider<ApplicationSettings>(options =>
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
            ConfigureAutoMapper();

            app.UseLykkeConfiguration(options =>
            {
                options.SwaggerOptions = _swaggerOptions;
            });
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
