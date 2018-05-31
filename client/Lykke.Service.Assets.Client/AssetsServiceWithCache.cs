using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client
{
    ///<inheritdoc/>
    internal class AssetsServiceWithCache : IAssetsServiceWithCache
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
        public Task<IReadOnlyCollection<AssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken())
            => _assetPairsCache.GetAll(cancellationToken);

        Task<IReadOnlyCollection<Asset>> IAssetsServiceWithCache.GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken())
            => GetAllAssetsAsync(false, cancellationToken);

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<Asset>> GetAllAssetsAsync(bool includeNonTradable, CancellationToken cancellationToken = new CancellationToken())
        {
            var items = await _assetsCache.GetAll(cancellationToken);

            return items
                .Where(x => x.IsTradable || includeNonTradable)
                .ToList();
        }

        ///<inheritdoc/>
        public Task<Asset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken())
            => _assetsCache.TryGet(assetId, cancellationToken);

        ///<inheritdoc/>
        public Task<AssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken())
            => _assetPairsCache.TryGet(assetPairId, cancellationToken);

        ///<inheritdoc/>
        public Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
            => _assetPairsCache.Reset(cancellationToken);

        ///<inheritdoc/>
        public Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
            => _assetsCache.Reset(cancellationToken);
    }
}
