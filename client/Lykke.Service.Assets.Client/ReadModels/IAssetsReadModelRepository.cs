using Lykke.Service.Assets.Client.Models.v3;
using System.Collections.Generic;

namespace Lykke.Service.Assets.Client.ReadModels
{
    /// <summary>
    /// Read-model repository for the Assets context.
    /// </summary>
    public interface IAssetsReadModelRepository
    {
        /// <summary>
        /// Get the asset by id. Returns null if not found.
        /// </summary>
        Asset TryGet(string id);

        /// <summary>
        /// Get all assets.
        /// </summary>
        IReadOnlyCollection<Asset> GetAll();
    }
}
