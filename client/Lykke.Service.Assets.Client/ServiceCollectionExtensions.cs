using System.Net.Http;
using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Models;
using Microsoft.Extensions.DependencyInjection;

namespace Lykke.Service.Assets.Client
{
    public static class ServiceCollectionExtensions
    {
        public static void RegisterAssetsClient(this IServiceCollection services, AssetServiceSettings settings)
        {
            services
                .AddSingleton<IAssetsService>(x => new AssetsService(settings.BaseUri, new HttpClient()));

            services
                .AddTransient<IAssetsServiceWithCache, AssetsServiceWithCache>();

            services
                .AddSingleton<IDictionaryCache<Asset>>(x => new DictionaryCache<Asset>(new DateTimeProvider(), settings.AssetsCacheExpirationPeriod));

            services
                .AddSingleton<IDictionaryCache<AssetPair>>(x => new DictionaryCache<AssetPair>(new DateTimeProvider(), settings.AssetPairsCacheExpirationPeriod));
        }
    }
}
