﻿using System;
using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories;
using Lykke.Service.Assets.Services;

namespace Lykke.Service.Assets.DependencyInjection
{
    public class ApiModule : Module
    {
        private readonly ApplicationSettings _settings;
        private readonly ILog _log;

        public ApiModule(ApplicationSettings settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log).SingleInstance();

            builder.RegisterInstance(_settings).SingleInstance();
            builder.RegisterInstance(_settings.AssetsService).SingleInstance();

            builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>();

            RegisterAssets(builder);
        }

        private void RegisterAssets(ContainerBuilder builder)
        {
            builder.Register(c => new AssetPairsRepository(
                    new AzureTableStorage<AssetPairEntity>(_settings.AssetsService.Dictionaries.DbConnectionString, "Dictionaries", _log)))
                .As<IAssetPairsRepository>();

            builder.RegisterType<AssetPairsCacheService>()
                .As<IAssetPairsCacheService>()
                .SingleInstance();

            builder.RegisterType<AssetPairsManager>()
                .As<IAssetPairsManager>()
                .WithParameter(new TypedParameter(typeof(TimeSpan), _settings.AssetsService.Dictionaries.CacheExpirationPeriod))
                .SingleInstance();
        }
    }
}