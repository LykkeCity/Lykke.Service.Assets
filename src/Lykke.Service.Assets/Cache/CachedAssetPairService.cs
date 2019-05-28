using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Cache
{
    public class CachedAssetPairService : ICachedAssetPairService
    {
        private readonly IAssetPairService _assetPairService;
        private readonly IDistributedCache<IAssetPair, AssetPair> _cache;
        private const string AllEntitiesKey = "All";

        public CachedAssetPairService(
            IAssetPairService assetPairService,
            IDistributedCache<IAssetPair, AssetPair> cache)
        {
            _assetPairService = assetPairService;
            _cache = cache;
        }

        public async Task<AssetPair> AddAsync(IAssetPair assetPair)
        {
            await InvalidateCache();

            return AutoMapper.Mapper.Map<AssetPair>(await _assetPairService.AddAsync(assetPair));
        }

        public AssetPair CreateDefault()
        {
            return AutoMapper.Mapper.Map<AssetPair>(_assetPairService.CreateDefault());
        }

        public async Task<IEnumerable<AssetPair>> GetAllAsync()
        {
            return await _cache.GetListAsync(AllEntitiesKey, 
                async () => AutoMapper.Mapper.Map<List<AssetPair>>(await _assetPairService.GetAllAsync()));
        }

        public async Task<AssetPair> GetAsync(string id)
        {
            return await _cache.GetAsync(id, 
                async  () => AutoMapper.Mapper.Map<AssetPair>(await _assetPairService.GetAsync(id)));
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
