using Autofac;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.RabbitSubscribers;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Responses.V2;
using Lykke.SettingsReader;
using StackExchange.Redis;

namespace Lykke.Service.Assets.Modules
{
    public class ApiModule : Module
    {
        private readonly IReloadingManager<ApplicationSettings> _settings;

        public ApiModule(
            IReloadingManager<ApplicationSettings> settings)
        {
            _settings = settings;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_settings).SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue.AssetsService).SingleInstance();

            RegisterCache<IAsset, AssetDto>(builder, "Assets");
            RegisterCache<IAssetCategory, AssetCategory>(builder, "AssetCategories");
            RegisterCache<IAssetPair, AssetPair>(builder, "AssetPairs");
            RegisterCache<IErc20Token, Erc20Token>(builder, "Erc20Tokens");

            RegisterRedis(builder);

            builder
               .RegisterType<CachedErc20TokenService>()
               .As<ICachedErc20TokenService>()
               .SingleInstance();

            builder
                .RegisterType<CachedAssetService>()
                .As<ICachedAssetService>()
                .SingleInstance();

            builder
                .RegisterType<CachedAssetCategoryService>()
                .As<ICachedAssetCategoryService>()
                .SingleInstance();

            builder
                .RegisterType<CachedAssetPairService>()
                .As<ICachedAssetPairService>()
                .SingleInstance();

            builder
                .RegisterType<CachedErc20TokenAssetService>()
                .As<ICachedErc20TokenAssetService>()
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
            System.Threading.ThreadPool.SetMinThreads(100, 100);
            var options = ConfigurationOptions.Parse(_settings.CurrentValue.AssetsService.RadisSettings.RedisConfiguration);
            options.ReconnectRetryPolicy = new ExponentialRetry(3000, 15000);

            var redis = ConnectionMultiplexer.Connect(options);

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

        private void RegisterCache<I, T>(ContainerBuilder builder, string partitionKey) where T : class, I
        {
            builder.RegisterType<DistributedCache<I, T>>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.AssetsService.Dictionaries.CacheExpirationPeriod))
                .WithParameter(TypedParameter.From($"{_settings.CurrentValue.AssetsService.RadisSettings.InstanceName}:v3:{partitionKey}"));
        }
    }
}
