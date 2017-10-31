using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client
{
    public interface IAssetsServiceWithCache
    {
        Task<IReadOnlyCollection<AssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken());

        Task<IReadOnlyCollection<Asset>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken());

        Task<Asset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());

        Task<AssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///    Forcibly updates client-side asset pairs cache
        /// </summary>
        Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        ///    Forcibly updates client-side assets cache
        /// </summary>
        Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}
