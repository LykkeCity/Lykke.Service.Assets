using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client.Custom
{
    public class CachedAssetsScervice : ICachedAssetsScervice
    {
        private readonly IAssetsservice _assetsservice;
        private readonly IDictionaryCache<AssetResponseModel> _assetsCache;
        private readonly IDictionaryCache<AssetPairResponseModel> _assetPairsCache;

        public CachedAssetsScervice(IAssetsservice assetsservice, IDictionaryCache<AssetResponseModel> assetsCache, IDictionaryCache<AssetPairResponseModel> assetPairsCache)
        {
            _assetsservice = assetsservice;
            _assetsCache = assetsCache;
            _assetPairsCache = assetPairsCache;
        }

        public async Task<AssetPairResponseModel> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => UpdateAssetPairsCacheAsync(cancellationToken));

            return _assetPairsCache.TryGet(assetPairId);
        }

        public async Task<IReadOnlyCollection<AssetPairResponseModel>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => UpdateAssetPairsCacheAsync(cancellationToken));

            return _assetPairsCache.GetAll();
        }

        public async Task<AssetResponseModel> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => UpdateAssetsCacheAsync(cancellationToken));
            
            return _assetsCache.TryGet(assetId);
        }

        public async Task<IReadOnlyCollection<AssetResponseModel>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => UpdateAssetsCacheAsync(cancellationToken));

            return _assetsCache.GetAll();
        }

        private async Task<IEnumerable<AssetResponseModel>> UpdateAssetsCacheAsync(CancellationToken cancellationToken)
        {
            return await _assetsservice.GetAssetsAsync(cancellationToken);
        }

        private async Task<IEnumerable<AssetPairResponseModel>> UpdateAssetPairsCacheAsync(CancellationToken cancellationToken)
        {
            return await _assetsservice.GetAssetPairsAsync(cancellationToken);
        }
    }
}