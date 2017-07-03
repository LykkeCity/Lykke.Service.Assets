using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Custom
{
    /// <summary>
    /// Provides assets data, cached on the client's side
    /// </summary>
    public interface ICachedAssetsService
    {
        Task<IAssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken());
        Task<IReadOnlyCollection<IAssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<IAsset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());
        Task<IReadOnlyCollection<IAsset>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken());
        
        /// <summary>
        /// Forcibly updates server-side and client-side asset pairs cache
        /// </summary>
        Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Forcibly updates server-side and client-side assets cache
        /// </summary>
        Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}