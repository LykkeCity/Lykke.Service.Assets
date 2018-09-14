using Lykke.Service.Assets.Client.Models.v3;
using System.Collections.Generic;

namespace Lykke.Service.Assets.Client.ReadModels
{
    /// <summary>
    /// Read-model for the AssetPairs context.
    /// </summary>
    public interface IAssetPairsReadModel
    {
        /// <summary>
        /// Get the asset-pair by id. Returns null if not found.
        /// </summary>
        AssetPair Get(string id);

        /// <summary>
        /// Get all asset-pairs.
        /// </summary>
        IReadOnlyCollection<AssetPair> GetAll();
    }
}
