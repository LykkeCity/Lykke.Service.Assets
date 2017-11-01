using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Tools;

namespace Lykke.Service.Assets.Managers
{
    public class AssetManager : IAssetManager
    {
        private readonly IAssetService  _assetService;
        private readonly ICache<IAsset> _cache;


        public AssetManager(
            IAssetService assetService,
            ICache<IAsset> cache)
        {
            _assetService = assetService;
            _cache        = cache;
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
            await InvalidateCache();

            await _assetService.DisableAsync(id);
        }

        public async Task EnableAsync(string id)
        {
            await InvalidateCache();

            await _assetService.EnableAsync(id);
        }

        public async Task<IEnumerable<IAsset>> GetAllAsync()
        {
            return await _cache.GetListAsync("All", _assetService.GetAllAsync);
        }

        public async Task<IEnumerable<IAsset>> GetAsync(string[] ids)
        {
            var listKey = ids.GetMD5();

            return await _cache.GetListAsync($"ForIdsList:{listKey}", () => _assetService.GetAsync(ids));
        }

        public async Task<IAsset> GetAsync(string id)
        {
            return await _cache.GetAsync(id, () => _assetService.GetAsync(id));
        }

        public async Task<IEnumerable<IAsset>> GetForCategoryAsync(string categoryId)
        {
            return await _cache.GetListAsync($"ForCategory:{categoryId}", () => _assetService.GetForCategoryAsync(categoryId));
        }

        public async Task InvalidateCache()
        {
            await _cache.InvalidateAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await InvalidateCache();

            await _assetService.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAsset asset)
        {
            await InvalidateCache();

            await _assetService.UpdateAsync(asset);
        }
    }
}
