using System;
using Common.Log;
using System.Net.Http;
using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.Assets.Client.Updaters;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Assets.Client
{
    /// <summary>
    /// Service registration for client asset services.
    /// </summary>
    public static class ServiceCollectionExtensions
    {
        /// <summary>
        /// Register the asset services.
        /// </summary>
        /// <param name="services">the dependency injection service collection</param>
        /// <param name="settings">the asset settings</param>
        /// <param name="log">the lykke log</param>
        /// <param name="autoRefresh">use expiring caches or use a self refreshing cache for the assets and asset-pairs</param>
        [Obsolete("Please, use the ContainerBuilderExtenions.RegisterAssetsClient method.")]
        public static void RegisterAssetsClient(this IServiceCollection services, AssetServiceSettings settings, ILog log, bool autoRefresh = true)
        {
            services
                .AddSingleton<IAssetsService>(x => new AssetsService(settings.BaseUri, new HttpClient()));

            services.AddTransient<IUpdater<Asset>>(x => new AssetsUpdater(x.GetService<IAssetsService>()));
            services.AddTransient(x => CreateDictionaryCache<Asset>(x, settings.AssetsCacheExpirationPeriod, log, autoRefresh));

            services.AddTransient<IUpdater<AssetPair>>(x => new AssetPairsUpdater(x.GetService<IAssetsService>()));
            services.AddTransient(x => CreateDictionaryCache<AssetPair>(x, settings.AssetPairsCacheExpirationPeriod, log, autoRefresh));

            services.AddTransient<IUpdater<Erc20Token>>(x => new Erc20TokensUpdater(x.GetService<IAssetsService>()));
            services.AddTransient(x => CreateDictionaryCache<Erc20Token>(x, settings.AssetPairsCacheExpirationPeriod, log, autoRefresh));

            services.AddSingleton<IAssetsServiceWithCache>(x => new AssetsServiceWithCache(
                    x.GetService<IDictionaryCache<Asset>>(),
                    x.GetService<IDictionaryCache<AssetPair>>(),
                    x.GetService<IDictionaryCache<Erc20Token>>()
                    ));
        }

        private static IDictionaryCache<T> CreateDictionaryCache<T>(IServiceProvider provider, TimeSpan period, ILog log, bool refreshing)
            where T : ICacheItem
        {
            if (refreshing)
            {
                return new RefreshingDictionaryCache<T>(
                    period,
                    provider.GetService<IUpdater<T>>(),
                    log
                );
            }

            return new ExpiringDictionaryCache<T>(
                period,
                provider.GetService<IUpdater<T>>()
            );
        }
    }
}
