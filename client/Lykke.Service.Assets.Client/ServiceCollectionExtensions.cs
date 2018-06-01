using System.Net.Http;
using Common.Log;
using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Assets.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAssetsClient(this IServiceCollection services, AssetServiceSettings settings,
            ILog log)
        {
            services
                .AddSingleton<IAssetsService>(x => new AssetsService(settings.BaseUri, new HttpClient()));

            services
                .AddSingleton<IDictionaryCache<Asset>>(x =>
                    new DictionaryCache<Asset>(new DateTimeProvider(), settings.AssetsCacheExpirationPeriod));

            services
                .AddSingleton<IDictionaryCache<AssetPair>>(x =>
                    new DictionaryCache<AssetPair>(new DateTimeProvider(), settings.AssetPairsCacheExpirationPeriod));

            services
                .AddSingleton<IAssetsServiceWithCache>(x => new AssetsServiceWithCache(
                    x.GetService<IAssetsService>(),
                    x.GetService<IDictionaryCache<Asset>>(),
                    x.GetService<IDictionaryCache<AssetPair>>(),
                    log
                ));
        }
    }
}
