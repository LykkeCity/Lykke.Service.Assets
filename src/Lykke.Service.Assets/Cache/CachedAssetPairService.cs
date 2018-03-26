using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Cache
{
    public class CachedAssetPairService : ICachedAssetPairService
    {
        private readonly IAssetPairService _assetPairService;
        private readonly DistributedCache<IAssetPair, AssetPairEntity> _cache;
        private const string AllEntitiesKey = "All";

        public CachedAssetPairService(
            IAssetPairService assetPairService,
            DistributedCache<IAssetPair, AssetPairEntity> cache)
        {
            _assetPairService = assetPairService;
            _cache = cache;
        }

        public async Task<IAssetPair> AddAsync(IAssetPair assetPair)
        {
            await InvalidateCache();

            return await _assetPairService.AddAsync(assetPair);
        }

        public IAssetPair CreateDefault()
        {
            return _assetPairService.CreateDefault();
        }

        public async Task<IEnumerable<IAssetPair>> GetAllAsync()
        {
            return await _cache.GetListAsync(AllEntitiesKey, _assetPairService.GetAllAsync);
        }

        public async Task<IAssetPair> GetAsync(string id)
        {
            return await _cache.GetAsync(id, () => _assetPairService.GetAsync(id));
        }

        public async Task RemoveAsync(string id)
        {
            await InvalidateCache(id);

            await _assetPairService.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAssetPair assetPair)
        {
            await InvalidateCache(assetPair.Id);

            await _assetPairService.UpdateAsync(assetPair);
        }

        private async Task InvalidateCache(string id = null)
        {
            if (id != null)
            {
                await _cache.RemoveAsync(id);
            }
            await _cache.RemoveAsync(AllEntitiesKey);
        }
    }
}
