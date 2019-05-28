using System;
using Autofac;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Responses.V2;
using Lykke.Service.Assets.Services;
using Lykke.Service.Assets.Settings;
using Lykke.SettingsReader;
using StackExchange.Redis;
using AssetPair = Lykke.Service.Assets.Responses.V2.AssetPair;
using Erc20Token = Lykke.Service.Assets.Responses.V2.Erc20Token;

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

            builder
                .RegisterType<CachedAssetConditionsService>()
                .As<ICachedAssetConditionsService>()
                .SingleInstance();

            RegisterRedis(builder);

            RegisterCache<IAsset, AssetDto>(builder, "Assets");
            RegisterCache<IAssetCategory, AssetCategory>(builder, "AssetCategories");
            RegisterCache<IAssetPair, AssetPair>(builder, "AssetPairs");
            RegisterCache<IErc20Token, Erc20Token>(builder, "Erc20Tokens");
            RegisterCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto>(builder, "AssetDefaultConditionLayer");
            RegisterCache<IAssetCondition, AssetConditionDto>(builder, "AssetConditions");
            RegisterCache<IAssetDefaultCondition, AssetDefaultConditionDto>(builder, "AssetDefaultCondition");

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
            options.ClientName = "Lykke.Service.Assets";
            
            builder.Register(c =>
                {
                    var lazy = new Lazy<ConnectionMultiplexer>(() => ConnectionMultiplexer.Connect(options));
                    return lazy.Value;
                })
                .As<IConnectionMultiplexer>()
                .SingleInstance();

            builder.Register(c => c.Resolve<IConnectionMultiplexer>().GetDatabase())
                .As<IDatabase>();

            builder.Register(c =>
            {
                var redis = c.Resolve<IConnectionMultiplexer>();
                return redis.GetServer(redis.GetEndPoints()[0]);
            });
        }

        private void RegisterCache<I, T>(ContainerBuilder builder, string partitionKey) where T : class, I
        {
            builder.RegisterType<DistributedCache<I, T>>()
                .As<IDistributedCache<I, T>>()
                .WithParameter(TypedParameter.From(_settings.RedisSettings.Expiration))
                .WithParameter(TypedParameter.From($"{_settings.RedisSettings.Instance}:v3:{partitionKey}"))
                .SingleInstance();
        }
    }
}
