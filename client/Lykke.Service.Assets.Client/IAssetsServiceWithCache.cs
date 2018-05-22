using Lykke.Service.Assets.Client.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client
{
    /// <summary>
    /// Client side in-memory cached version of the <see cref="IAssetsService"/>.
    /// </summary>
    public interface IAssetsServiceWithCache
    {
        /// <summary>
        /// Get all asset-pairs.
        /// </summary>
        Task<IReadOnlyCollection<AssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Get all assets
        /// </summary>
        [Obsolete("Use GetAllAssetsAsync(bool) instead")]
        Task<IReadOnlyCollection<Asset>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Get all assets
        /// </summary>
        Task<IReadOnlyCollection<Asset>> GetAllAssetsAsync(bool includeNonTradable, CancellationToken cancellationToken = new CancellationToken());
        
        /// <summary>
        /// Try to find an asset with given id.
        /// </summary>
        Task<Asset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Try to find an asset-pair with given id.
        /// </summary>
        Task<AssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///    Forcibly updates client-side asset-pairs cache
        /// </summary>
        Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///    Forcibly updates client-side assets cache
        /// </summary>
        Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Starts an automatic update process that will keep the caches updated the background.
        /// </summary>
        /// <returns>the update process, when disposed the auto update will stop</returns>
        IDisposable StartAutoCacheUpdate();
    }
}
