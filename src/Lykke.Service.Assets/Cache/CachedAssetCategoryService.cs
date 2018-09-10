using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;

namespace Lykke.Service.Assets.Cache
{
    public class CachedAssetCategoryService : ICachedAssetCategoryService
    {
        private readonly IAssetCategoryService _assetCategoryService;
        private readonly DistributedCache<IAssetCategory, AssetCategory> _cache;
        private const string AllEntitiesKey = "All";

        public CachedAssetCategoryService(
            IAssetCategoryService assetCategoryService,
            DistributedCache<IAssetCategory, AssetCategory> cache)
        {
            _assetCategoryService = assetCategoryService;
            _cache = cache;
        }

        public async Task<AssetCategory> AddAsync(IAssetCategory assetCategory)
        {
            await InvalidateCache();

            return AutoMapper.Mapper.Map<AssetCategory>(await _assetCategoryService.AddAsync(assetCategory));
        }

        public Task<AssetCategory> GetAsync(string id)
        {
            return _cache.GetAsync(id, 
                async () => AutoMapper.Mapper.Map<AssetCategory>(await _assetCategoryService.GetAsync(id)));
        }

        public Task<IEnumerable<AssetCategory>> GetAllAsync()
        {
            return _cache.GetListAsync(AllEntitiesKey,
                async () => AutoMapper.Mapper.Map<List<AssetCategory>>(await _assetCategoryService.GetAllAsync()));
        }

        public async Task RemoveAsync(string id)
        {
            await InvalidateCache(id);

            await _assetCategoryService.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAssetCategory assetCategory)
        {
            await InvalidateCache(assetCategory.Id);

            await _assetCategoryService.UpdateAsync(assetCategory);
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
