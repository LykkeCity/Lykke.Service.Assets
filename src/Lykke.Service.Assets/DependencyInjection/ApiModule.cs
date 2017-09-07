using System;
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
            RegisterAssetPairs(builder);
            RegisterAssetAttributes(builder);
            RegisterIssuerRepository(builder);
            RegisterAssetExtendedInfoRepository(builder);
        }

        private void RegisterAssetExtendedInfoRepository(ContainerBuilder builder)
        {
            builder.Register(c => new AssetExtendedInfoRepository(
                   new AzureTableStorage<AssetExtendedInfoEntity>(_settings.AssetsService.Dictionaries.DbConnectionString, "Dictionaries", _log)))
               .As<IDictionaryRepository<IAssetExtendedInfo>>();

            RegisterDictionaryManager<IAssetExtendedInfo>(builder);
        }

        private void RegisterIssuerRepository(ContainerBuilder builder)
        {
            builder.Register(c => new IssuerRepository(
                   new AzureTableStorage<IssuerEntity>(_settings.AssetsService.Dictionaries.DbConnectionString, "AssetIssuers", _log)))
               .As<IDictionaryRepository<IIssuer>>();

            RegisterDictionaryManager<IIssuer>(builder);
        }

        private void RegisterDictionaryManager<T>(ContainerBuilder builder) where T : IDictionaryItem
        {
            builder.RegisterType<DictionaryCacheService<T>>()
                .As<IDictionaryCacheService<T>>()
                .SingleInstance();

            builder.RegisterType<DictionaryManager<T>>()
                .As<IDictionaryManager<T>>()
                .WithParameter(new TypedParameter(typeof(TimeSpan), _settings.AssetsService.Dictionaries.CacheExpirationPeriod))
                .SingleInstance();
        }

        private void RegisterAssetAttributes(ContainerBuilder builder)
        {
            builder.Register(c => new AssetAttributesRepository(
                   new AzureTableStorage<AssetAttributesEntity>(_settings.AssetsService.Dictionaries.DbConnectionString, "AssetAttributes", _log)))
               .As<IDictionaryRepository<IAssetAttributes>>();

            builder.RegisterType<DictionaryCacheService<IAssetAttributes>>()
                .As<IDictionaryCacheService<IAssetAttributes>>()
                .SingleInstance();

            builder.RegisterType<DictionaryManager<IAssetAttributes>>()
                .As<IDictionaryManager<IAssetAttributes>>()
                .WithParameter(new TypedParameter(typeof(TimeSpan), _settings.AssetsService.Dictionaries.CacheExpirationPeriod))
                .SingleInstance();
        }

        private void RegisterAssets(ContainerBuilder builder)
        {
            builder.Register(c => new AssetsRepository(
                    new AzureTableStorage<AssetEntity>(_settings.AssetsService.Dictionaries.DbConnectionString, "Dictionaries", _log)))
                .As<IDictionaryRepository<IAsset>>();

            builder.RegisterType<DictionaryCacheService<IAsset>>()
                .As<IDictionaryCacheService<IAsset>>()
                .SingleInstance();

            builder.RegisterType<DictionaryManager<IAsset>>()
                .As<IDictionaryManager<IAsset>>()
                .WithParameter(new TypedParameter(typeof(TimeSpan), _settings.AssetsService.Dictionaries.CacheExpirationPeriod))
                .SingleInstance();
        }

        private void RegisterAssetPairs(ContainerBuilder builder)
        {
            builder.Register(c => new AssetPairsRepository(
                    new AzureTableStorage<AssetPairEntity>(_settings.AssetsService.Dictionaries.DbConnectionString, "Dictionaries", _log)))
                .As<IDictionaryRepository<IAssetPair>>();

            builder.RegisterType<DictionaryCacheService<IAssetPair>>()
                .As<IDictionaryCacheService<IAssetPair>>()
                .SingleInstance();

            builder.RegisterType<DictionaryManager<IAssetPair>>()
                .As<IDictionaryManager<IAssetPair>>()
                .WithParameter(new TypedParameter(typeof(TimeSpan), _settings.AssetsService.Dictionaries.CacheExpirationPeriod))
                .SingleInstance();
        }
    }
}