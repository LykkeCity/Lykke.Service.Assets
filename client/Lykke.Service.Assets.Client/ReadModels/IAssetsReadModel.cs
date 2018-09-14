using Lykke.Service.Assets.Client.Models.v3;
using System.Collections.Generic;

namespace Lykke.Service.Assets.Client.ReadModels
{
    /// <summary>
    /// Read-model for the Assets context.
    /// </summary>
    public interface IAssetsReadModel
    {
        /// <summary>
        /// Get the asset by id. Returns null if not found.
        /// </summary>
        Asset Get(string id);

        /// <summary>
        /// Get all assets.
        /// </summary>
        IReadOnlyCollection<Asset> GetAll();
    }
}
