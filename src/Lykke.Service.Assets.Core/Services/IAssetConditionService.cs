using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    /// <summary>
    /// Service for work with set of settings for the asset, rules\conditions of use of the asset.
    /// </summary>
    public interface IAssetConditionService
    {
        Task<IEnumerable<IAssetConditionLayer>> GetLayersAsync();

        Task<IAssetConditionLayer> GetLayerAsync(string layerId);

        Task AddAssetConditionAsync(string layerId, IAssetCondition assetCondition);

        Task UpdateAssetConditionAsync(string layerId, IAssetCondition assetCondition);

        Task DeleteAssetConditionAsync(string layerId, string assetId);

        Task AddLayerAsync(IAssetConditionLayer layer);

        Task UpdateLayerAsync(IAssetConditionLayer layer);

        Task DeleteLayerAsync(string layerId);

        Task<IEnumerable<IAssetConditionLayer>> GetClientLayers(string clientId);

        Task AddClientLayerAsync(string clientId, string layerId);

        Task RemoveClientLayerAsync(string clientId, string layerId);

        /// <summary>
        /// Get actual asset conditions for client.
        /// Service aggregate all assign layer for clienе and return final state of settings
        /// </summary>
        /// <param name="clientId">client identity</param>
        /// <returns>A collection of asset conditions.</returns>
        Task<IEnumerable<IAssetCondition>> GetAssetConditionsByClient(string clientId);

        /// <summary>
        /// Get actual asset conditions settings for all assets for client.
        /// Service aggregate all assign layer for clienе and return final state of settings
        /// </summary>
        /// <param name="clientId">client identity</param>
        Task<IAssetConditionLayerSettings> GetAssetConditionsLayerSettingsByClient(string clientId);
    }
}
