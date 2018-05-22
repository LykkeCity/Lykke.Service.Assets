using Lykke.Service.Assets.Client.Cache;
using Lykke.Service.Assets.Client.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Common.Log;

namespace Lykke.Service.Assets.Client
{
    ///<inheritdoc/>
    public class AssetsServiceWithCache : IAssetsServiceWithCache
    {
        private readonly IAssetsService _assetsService;
        private readonly IDictionaryCache<Asset> _assetsCache;
        private readonly IDictionaryCache<AssetPair> _assetPairsCache;
        private readonly ILog _log;

        ///<inheritdoc/>
        public AssetsServiceWithCache(IAssetsService assetsService, IDictionaryCache<Asset> assetsCache, IDictionaryCache<AssetPair> assetPairsCache, ILog log)
        {
            _assetsService = assetsService;
            _assetsCache = assetsCache;
            _assetPairsCache = assetPairsCache;
            _log = log;
        }

        ///<inheritdoc/>
        public async Task<IReadOnlyCollection<AssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetPairsAsync(cancellationToken));

            return _assetPairsCache.GetAll();
        }

        [Obsolete]
        public async Task<IReadOnlyCollection<Asset>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken())
            => await GetAllAssetsAsync(false, cancellationToken);

        public async Task<IReadOnlyCollection<Asset>> GetAllAssetsAsync(bool includeNonTradable, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetsAsync(cancellationToken));

            return _assetsCache.GetAll()
                .Where(x => x.IsTradable || includeNonTradable)
                .ToList();
        }

        ///<inheritdoc/>
        public async Task<Asset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetsAsync(cancellationToken));

            return _assetsCache.TryGet(assetId);
        }

        ///<inheritdoc/>
        public async Task<AssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetPairsAsync(cancellationToken));

            return _assetPairsCache.TryGet(assetPairId);
        }

        ///<inheritdoc/>
        public async Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _assetPairsCache.Update(await GetUncachedAssetPairsAsync(cancellationToken));
        }

        ///<inheritdoc/>
        public async Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            _assetsCache.Update(await GetUncachedAssetsAsync(cancellationToken));
        }

       private async Task<IEnumerable<Asset>> GetUncachedAssetsAsync(CancellationToken cancellationToken)
       {
           return await _assetsService.AssetGetAllAsync(true, cancellationToken);
       }

        private async Task<IEnumerable<AssetPair>> GetUncachedAssetPairsAsync(CancellationToken cancellationToken)
        {
            return await _assetsService.AssetPairGetAllAsync(cancellationToken);
        }
    }
}
