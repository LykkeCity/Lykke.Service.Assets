using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Managers
{
    public class AssetPairManager : IAssetPairManager
    {
        private readonly IAssetPairService  _assetPairService;
        private readonly ICache<IAssetPair> _cache;


        public AssetPairManager(
            IAssetPairService assetPairService,
            ICache<IAssetPair> cache)
        {
            _assetPairService = assetPairService;
            _cache            = cache;
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
            return await _cache.GetListAsync("All", _assetPairService.GetAllAsync);
        }

        public async Task<IAssetPair> GetAsync(string id)
        {
            return await _cache.GetAsync(id, () => _assetPairService.GetAsync(id));
        }

        public async Task InvalidateCache()
        {
            await _cache.InvalidateAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await InvalidateCache();

            await _assetPairService.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAssetPair assetPair)
        {
            await InvalidateCache();

            await _assetPairService.UpdateAsync(assetPair);
        }
    }
}
