using Autofac;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Responses.V2;
using Lykke.Service.Assets.Settings;
using Lykke.SettingsReader;
using StackExchange.Redis;

namespace Lykke.Service.Assets.Modules
{
    public class CacheModule : Module
    {
        private readonly ApplicationSettings.AssetsSettings _settings;

        public CacheModule(IReloadingManager<ApplicationSettings> settings)
        {
            _settings = settings.CurrentValue.AssetsService;
        }

        protected override void Load(ContainerBuilder builder)
        {
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

            RegisterRedis(builder);

            RegisterCache<IAsset, AssetDto>(builder, "Assets");
            RegisterCache<IAssetCategory, AssetCategory>(builder, "AssetCategories");
            RegisterCache<IAssetPair, AssetPair>(builder, "AssetPairs");
            RegisterCache<IErc20Token, Erc20Token>(builder, "Erc20Tokens");

            builder.RegisterType<AssetsForClientCacheManager>()
                .As<IAssetsForClientCacheManager>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.RedisSettings.Expiration))
                .WithParameter(TypedParameter.From($"{_settings.RedisSettings.Instance}:v3:ClientSettings"));
        }

        private void RegisterRedis(ContainerBuilder builder)
        {
            System.Threading.ThreadPool.SetMinThreads(300, 300);
            var options = ConfigurationOptions.Parse(_settings.RedisSettings.Configuration);
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

        private void RegisterCache<I, T>(ContainerBuilder builder, string partitionKey) where T : class, I
        {
            builder.RegisterType<DistributedCache<I, T>>()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.RedisSettings.Expiration))
                .WithParameter(TypedParameter.From($"{_settings.RedisSettings.Instance}:v3:{partitionKey}"));
        }
    }
}
