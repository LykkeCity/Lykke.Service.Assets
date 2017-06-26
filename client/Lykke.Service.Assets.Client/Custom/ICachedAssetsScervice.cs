using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client.Custom
{
    /// <summary>
    /// Provides assets data, cached on the client's side
    /// </summary>
    public interface ICachedAssetsScervice
    {
        Task<AssetPairResponseModel> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken());
        Task<IReadOnlyCollection<AssetPairResponseModel>> GetAssetPairs(CancellationToken cancellationToken = new CancellationToken());
        Task<AssetResponseModel> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());
        Task<IReadOnlyCollection<AssetResponseModel>> GetAssetsAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}