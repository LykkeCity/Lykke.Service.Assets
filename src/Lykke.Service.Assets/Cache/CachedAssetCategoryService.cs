using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Cache
{
    public class CachedAssetCategoryService : ICachedAssetCategoryService
    {
        private readonly IAssetCategoryService _assetCategoryService;
        private readonly DistributedCache<IAssetCategory, AssetCategoryEntity> _cache;
        private const string AllEntitiesKey = "All";

        public CachedAssetCategoryService(
            IAssetCategoryService assetCategoryService,
            DistributedCache<IAssetCategory, AssetCategoryEntity> cache)
        {
            _assetCategoryService = assetCategoryService;
            _cache = cache;
        }

        public async Task<IAssetCategory> AddAsync(IAssetCategory assetCategory)
        {
            await InvalidateCache();

            return await _assetCategoryService.AddAsync(assetCategory);
        }

        public async Task<IAssetCategory> GetAsync(string id)
        {
            return await _cache.GetAsync(id, () => _assetCategoryService.GetAsync(id));
        }

        public async Task<IEnumerable<IAssetCategory>> GetAllAsync()
        {
            return await _cache.GetListAsync(AllEntitiesKey, () => _assetCategoryService.GetAllAsync());
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
