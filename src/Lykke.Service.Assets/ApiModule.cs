using Autofac;
using Common.Log;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Managers;
using Lykke.Service.Assets.RabbitSubscribers;
using Lykke.SettingsReader;
using StackExchange.Redis;

namespace Lykke.Service.Assets
{
    public class ApiModule : Module
    {
        private readonly IReloadingManager<ApplicationSettings> _settings;
        private readonly ILog                                   _log;

        public ApiModule(
            IReloadingManager<ApplicationSettings> settings,
            ILog log)
        {
            _settings = settings;
            _log      = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log).SingleInstance();

            builder.RegisterInstance(_settings).SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue.AssetsService).SingleInstance();

            RegisterCache<IAsset>(builder, "Assets");
            RegisterCache<IAssetCategory>(builder, "AssetCategories");
            RegisterCache<IAssetPair>(builder, "AssetPairs");
            RegisterCache<IErc20Token>(builder, "Erc20Tokens");

            RegisterRedis(builder);

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder
               .RegisterType<Erc20TokenManager>()
               .As<IErc20TokenManager>()
               .SingleInstance();

            builder
                .RegisterType<AssetManager>()
                .As<IAssetManager>()
                .SingleInstance();

            builder
                .RegisterType<AssetCategoryManager>()
                .As<IAssetCategoryManager>()
                .SingleInstance();

            builder
                .RegisterType<AssetPairManager>()
                .As<IAssetPairManager>()
                .SingleInstance();

            builder
                .RegisterType<Erc20TokenAssetManager>()
                .As<IErc20TokenAssetManager>()
                .SingleInstance();

            RegisterRabbitMqSubscribers(builder);

            builder.RegisterInstance(_settings.CurrentValue.AssetsService.RadisSettings)
                .As<IAssetsForClientCacheManagerSettings>()
                .SingleInstance();

            builder.RegisterType<AssetsForClientCacheManager>()
                .As<IAssetsForClientCacheManager>()
                .SingleInstance();
        }

        private void RegisterRedis(ContainerBuilder builder)
        {
            var redis = ConnectionMultiplexer.Connect(_settings.CurrentValue.AssetsService.RadisSettings.RedisConfiguration);

            builder.RegisterInstance(redis).SingleInstance();
            builder.Register(
                c =>
                    c.Resolve<ConnectionMultiplexer>()
                        .GetServer(redis.GetEndPoints()[0]));

            builder.Register(
                c =>
                    c.Resolve<ConnectionMultiplexer>()
                        .GetDatabase());
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            // TODO: You should register each subscriber in DI container as IStartable singleton and autoactivate it

            builder.RegisterType<ErcContractSubscriber>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.AssetsService.Rabbit.ConnectionString));
        }

        private void RegisterCache<T>(ContainerBuilder builder, string partitionKey)
        {
            builder.RegisterType<Cache<T>>()
                .As<ICache<T>>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.AssetsService.Dictionaries.CacheExpirationPeriod))
                .WithParameter(TypedParameter.From(partitionKey));
        }
    }
}
