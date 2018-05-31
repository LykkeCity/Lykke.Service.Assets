using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client
{
    ///<inheritdoc/>
    public class AssetsServiceWithCache : IAssetsServiceWithCache
    {
        private readonly IDictionaryCache<Asset> _assetsCache;
        private readonly IDictionaryCache<AssetPair> _assetPairsCache;

        ///<inheritdoc/>
        public AssetsServiceWithCache(IDictionaryCache<Asset> assetsCache, IDictionaryCache<AssetPair> assetPairsCache)
        {
            _assetsCache = assetsCache;
            _assetPairsCache = assetPairsCache;
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<AssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken()) 
            => await _assetPairsCache.GetAll(cancellationToken);

        async Task<IReadOnlyCollection<Asset>> IAssetsServiceWithCache.GetAllAssetsAsync(CancellationToken cancellationToken)
            => await GetAllAssetsAsync(false, cancellationToken);

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<Asset>> GetAllAssetsAsync(bool includeNonTradable, CancellationToken cancellationToken = new CancellationToken()) 
            => await _assetsCache.GetAll(cancellationToken);

        ///<inheritdoc/>
        public async Task<Asset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken()) 
            => await _assetsCache.TryGet(assetId, cancellationToken);

        ///<inheritdoc/>
        public async Task<AssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken()) 
            => await _assetPairsCache.TryGet(assetPairId, cancellationToken);

        ///<inheritdoc/>
        public async Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken()) 
            => await _assetPairsCache.Reset(cancellationToken);

        ///<inheritdoc/>
        public async Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken()) 
            => await _assetsCache.Reset(cancellationToken);
    }
}
