using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client.Custom
{
    public class CachedAssetsService : ICachedAssetsService
    {
        private readonly IAssetsservice _assetsservice;
        private readonly IDictionaryCache<AssetResponseModel> _assetsCache;
        private readonly IDictionaryCache<AssetPairResponseModel> _assetPairsCache;

        public CachedAssetsService(IAssetsservice assetsservice, IDictionaryCache<AssetResponseModel> assetsCache, IDictionaryCache<AssetPairResponseModel> assetPairsCache)
        {
            _assetsservice = assetsservice;
            _assetsCache = assetsCache;
            _assetPairsCache = assetPairsCache;
        }

        public async Task<IAssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetPairsAsync(cancellationToken));

            return _assetPairsCache.TryGet(assetPairId);
        }

        public async Task<IReadOnlyCollection<IAssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetPairsAsync(cancellationToken));

            return _assetPairsCache.GetAll();
        }

        public async Task<IAsset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetsAsync(cancellationToken));
            
            return _assetsCache.TryGet(assetId);
        }

        public async Task<IReadOnlyCollection<IAsset>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetsAsync(cancellationToken));

            return _assetsCache.GetAll();
        }

        public async Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsservice.UpdateAssetPairsCacheAsync(cancellationToken);

            _assetPairsCache.Update(await GetUncachedAssetPairsAsync(cancellationToken));
        }

        public async Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsservice.UpdateAssetsCacheAsync(cancellationToken);

            _assetsCache.Update(await GetUncachedAssetsAsync(cancellationToken));
        }

        private async Task<IEnumerable<AssetResponseModel>> GetUncachedAssetsAsync(CancellationToken cancellationToken)
        {
            return await _assetsservice.GetAssetsAsync(cancellationToken);
        }

        private async Task<IEnumerable<AssetPairResponseModel>> GetUncachedAssetPairsAsync(CancellationToken cancellationToken)
        {
            return await _assetsservice.GetAssetPairsAsync(cancellationToken);
        }
    }
}