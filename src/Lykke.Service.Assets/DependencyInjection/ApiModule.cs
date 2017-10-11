using System;
using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.Service.Assets.Services;
using AzureStorage.Tables.Templates.Index;

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
            //builder.RegisterType<AssetServiceHelper>().As<IAssetsServiceHelper>().SingleInstance();

            //builder.RegisterType<DateTimeProvider>().As<IDateTimeProvider>();

            //RegisterAssets(builder);
            //RegisterAssetPairs(builder);
            //RegisterAssetAttributes(builder);
            //RegisterIssuerRepository(builder);
            //RegisterAssetExtendedInfoRepository(builder);
            //RegisterAssetCategoryRepository(builder);
            //RegisterAssetGroupsRepository(builder);
            //
            //RegisterServicesForJobs(builder);
            //RegisterRepositoriesForJobs(builder);
            //RegisterRabbitMqSubscribers(builder);
        }

        /*
        private void RegisterAssetGroupsRepository(ContainerBuilder builder)
        {
            builder.RegisterInstance<IAssetGroupRepository>(
                new AssetGroupRepository(AzureTableStorage<AssetGroupEntity>.Create(() => _settings.AssetsService.Db.ClientPersonalInfoConnString,
                    "AssetGroups", _log)));

            RegisterDictionaryManager<IAssetGroup>(builder);
        }

        private void RegisterAssetCategoryRepository(ContainerBuilder builder)
        {
            builder.RegisterInstance<IDictionaryRepository<IAssetCategory>>(
                new AssetCategoryRepository(AzureTableStorage<AssetCategoryEntity>.Create(() => _settings.AssetsService.Dictionaries.DbConnectionString,
                    "AssetCategories", _log)));

            RegisterDictionaryManager<IAssetCategory>(builder);
        }

        private void RegisterAssetExtendedInfoRepository(ContainerBuilder builder)
        {
            builder.RegisterInstance<IDictionaryRepository < IAssetDescription >> (
                new AssetExtendedInfoRepository(AzureTableStorage<AssetExtendedInfoEntity>.Create(() => _settings.AssetsService.Dictionaries.DbConnectionString,
                    "Dictionaries", _log)));

            RegisterDictionaryManager<IAssetDescription>(builder);
        }

        private void RegisterIssuerRepository(ContainerBuilder builder)
        {

            builder.RegisterInstance<IDictionaryRepository<IIssuer>>(
                new IssuerRepository(AzureTableStorage<IssuerEntity>.Create(() => _settings.AssetsService.Dictionaries.DbConnectionString,
                    "AssetIssuers", _log)));

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
            builder.RegisterInstance<IDictionaryRepository<IAssetAttributes>>(
               new AssetAttributeRepository(AzureTableStorage<AssetAttributeEntity>.Create(() => _settings.AssetsService.Dictionaries.DbConnectionString,
                   "AssetAttributes", _log)));

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
            builder.Register(c => new AssetRepository(
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

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            // TODO: You should register each subscriber in DI container as IStartable singleton and autoactivate it

            builder.RegisterType<ErcContractSubscriber>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.AssetsService.Rabbit.ConnectionString));
        }
        */
    }
}