using System;
using System.Net.Http;
using Autofac;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Client
{
    /// <summary>
    /// Service registration for client asset services.
    /// </summary>
    [UsedImplicitly]
    public static class ContainerBuilderExtenions
    {
        /// <summary>
        /// Register the asset services.
        /// </summary>
        /// <param name="builder">The container builder for adding the services to.</param>
        /// <param name="settings">The asset settings.</param>
        /// <param name="autoRefresh">The marker of an expiring caches or a self refreshing cache for the assets and asset-pairs usage.</param>
        [UsedImplicitly]
        public static void RegisterAssetsClient(this ContainerBuilder builder, AssetServiceSettings settings,
            bool autoRefresh = true)
        {
            builder.Register(x => new AssetsService(settings.BaseUri, new HttpClient()))
                .As<IAssetsService>()
                .SingleInstance();

            // ---
            builder.Register(x => new AssetsUpdater(x.Resolve<IAssetsService>()))
                .As<IUpdater<Asset>>()
                .InstancePerDependency(); // These calls are optional, just for clarification purpose.
            builder.Register(x =>
                    CreateDictionaryCache<Asset>(x, settings.AssetsCacheExpirationPeriod, x.Resolve<ILogFactory>(),
                        autoRefresh)
                )
                .InstancePerDependency();

            // ---
            builder.Register(x => new AssetPairsUpdater(x.Resolve<IAssetsService>()))
                .As<IUpdater<AssetPair>>()
                .InstancePerDependency();
            builder.Register(x => 
                    CreateDictionaryCache<AssetPair>(x, settings.AssetPairsCacheExpirationPeriod,
                        x.Resolve<ILogFactory>(), autoRefresh)
                )
                .InstancePerDependency();

            // ---
            builder.Register(x => new AssetsServiceWithCache(
                    x.Resolve<IDictionaryCache<Asset>>(),
                    x.Resolve<IDictionaryCache<AssetPair>>())
                )
                .As<IAssetsServiceWithCache>()
                .SingleInstance();
        }

        private static IDictionaryCache<T> CreateDictionaryCache<T>(IComponentContext context, TimeSpan period, ILogFactory logFactory, bool refreshing)
            where T : ICacheItem
        {
            if (refreshing)
            {
                return new RefreshingDictionaryCache<T>(
                    period,
                    context.Resolve<IUpdater<T>>(),
                    logFactory.CreateLog(nameof(RefreshingDictionaryCache<T>))
                );
            }

            return new ExpiringDictionaryCache<T>(
                period,
                context.Resolve<IUpdater<T>>()
            );
        }
    }
}
