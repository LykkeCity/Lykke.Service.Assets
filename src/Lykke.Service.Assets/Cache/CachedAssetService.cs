using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.DTOs;

namespace Lykke.Service.Assets.Cache
{
    public class CachedAssetService : ICachedAssetService
    {
        private readonly IAssetService _assetService;
        private readonly DistributedCache<IAsset, AssetDto> _cache;
        private const string AllEntitiesKey = "All";
        private const string AllTradableKey = "AllTradable";

        public CachedAssetService(
            IAssetService assetService,
            DistributedCache<IAsset, AssetDto> cache)
        {
            _assetService = assetService;
            _cache = cache;
        }

        public async Task<IAsset> AddAsync(IAsset asset)
        {
            await InvalidateCache();

            return await _assetService.AddAsync(asset);
        }

        public IAsset CreateDefault()
        {
            return _assetService.CreateDefault();
        }

        public async Task DisableAsync(string id)
        {
            await InvalidateCache(id);

            await _assetService.DisableAsync(id);
        }

        public async Task EnableAsync(string id)
        {
            await InvalidateCache(id);

            await _assetService.EnableAsync(id);
        }

        public async Task<IEnumerable<IAsset>> GetAllAsync(bool includeNonTradable)
        {
            var listKey = includeNonTradable ? AllEntitiesKey : AllTradableKey;

            return await _cache.GetListAsync(listKey, () => _assetService.GetAllAsync(includeNonTradable));
        }

        public async Task<IEnumerable<IAsset>> GetAsync(string[] ids, bool? isTradable)
        {
            var assets = await GetAllAsync(isTradable ?? true);
            if (ids.Any())
            {
                assets = assets.Where(x => ids.Contains(x.Id));
            }

            return assets;
        }

        public async Task<IAsset> GetAsync(string id)
        {
            return await _cache.GetAsync(id, () => _assetService.GetAsync(id));
        }

        public async Task<IEnumerable<IAsset>> GetForCategoryAsync(string categoryId)
        {
            var assets = await GetAllAsync(true);
            return assets.Where(x => x.CategoryId == categoryId);
        }

        public async Task RemoveAsync(string id)
        {
            await InvalidateCache(id);

            await _assetService.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAsset asset)
        {
            await InvalidateCache(asset.Id);

            await _assetService.UpdateAsync(asset);
        }

        private async Task InvalidateCache(string id = null)
        {
            if (id != null)
            {
                await _cache.RemoveAsync(id);
            }
            await _cache.RemoveAsync(AllEntitiesKey);
            await _cache.RemoveAsync(AllTradableKey);
        }
    }
}
