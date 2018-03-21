using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.DTOs;

namespace Lykke.Service.Assets.Cache
{
    public class CachedErc20TokenAssetService : ICachedErc20TokenAssetService
    {
        private readonly IErc20TokenAssetService _tokenAssetService;
        private readonly DistributedCache<IAsset, AssetDto> _cache;
        private const string AllEntitiesKey = "All";

        public CachedErc20TokenAssetService(
            IErc20TokenAssetService tokenAssetService,
            DistributedCache<IAsset, AssetDto> cache)
        {
            _tokenAssetService = tokenAssetService;
            _cache = cache;
        }

        public async Task<IAsset> CreateAssetForErc20TokenAsync(string tokenAddress)
        {
            await InvalidateCache();

            return await _tokenAssetService.CreateAssetForErc20TokenAsync(tokenAddress);
        }

        private async Task InvalidateCache()
        {
            await _cache.RemoveAsync(AllEntitiesKey);
        }
    }
}
