using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Managers
{
    public class AssetCategoryManager : IAssetCategoryManager
    {
        private readonly IAssetCategoryService  _assetCategoryService;
        private readonly ICache<IAssetCategory> _cache;


        public AssetCategoryManager(
            IAssetCategoryService assetCategoryService,
            ICache<IAssetCategory> cache)
        {
            _assetCategoryService = assetCategoryService;
            _cache                = cache;
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
            return await _cache.GetListAsync("All", _assetCategoryService.GetAllAsync);
        }

        public async Task InvalidateCache()
        {
            await _cache.InvalidateAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await InvalidateCache();

            await _assetCategoryService.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAssetCategory assetCategory)
        {
            await InvalidateCache();

            await _assetCategoryService.UpdateAsync(assetCategory);
        }
    }
}
