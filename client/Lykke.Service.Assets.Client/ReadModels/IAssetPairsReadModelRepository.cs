using Lykke.Service.Assets.Client.Models.v3;
using System.Collections.Generic;

namespace Lykke.Service.Assets.Client.ReadModels
{
    /// <summary>
    /// Read-model repository for the AssetPairs context.
    /// </summary>
    public interface IAssetPairsReadModelRepository
    {
        /// <summary>
        /// Get the asset-pair by id. Returns null if not found.
        /// </summary>
        AssetPair TryGet(string id);

        /// <summary>
        /// Get all asset-pairs.
        /// </summary>
        IReadOnlyCollection<AssetPair> GetAll();
    }
}
