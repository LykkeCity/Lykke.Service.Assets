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
        /// <summary>
        /// Get actual asset conditions for client.
        /// Service aggregate all assign layer for clienе and return final state of settings
        /// </summary>
        /// <param name="clientId">client identity</param>
        /// <returns>Dictionary by asset with asset conditions state</returns>
        Task<IReadOnlyDictionary<string, IAssetCondition>> GetAssetConditionsByClient(string clientId);

        /// <summary>
        /// Get actual asset conditions settings for all assets for client.
        /// Service aggregate all assign layer for clienе and return final state of settings
        /// </summary>
        /// <param name="clientId">client identity</param>
        Task<IAssetConditionLayerSettings> GetAssetConditionsLayerSettingsByClient(string clientId);
    }
}
