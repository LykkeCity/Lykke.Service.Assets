using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionRepository
    {
        /// <summary>
        /// Returns asset conditions by layer id.
        /// </summary>
        /// <param name="layerId">The asset condition layer id.</param>
        /// <returns>A collection of asset conditions.</returns>
        Task<IEnumerable<IAssetCondition>> GetAsync(string layerId);

        /// <summary>
        /// Returns asset conditions for each layer.
        /// </summary>
        /// <param name="layers">The collection of layers id.</param>
        /// <returns>A collection of asset conditions.</returns>
        Task<IEnumerable<IAssetCondition>> GetAsync(IEnumerable<string> layers);

        /// <summary>
        /// Inserts a new entity if it does not exist, otherwise updates existing entity.
        /// </summary>
        /// <param name="layerId">The id of the layer that contains asset condition.</param>
        /// <param name="assetCondition">The asset conditons.</param>
        /// <returns></returns>
        Task InsertOrUpdateAsync(string layerId, IAssetCondition assetCondition);

        /// <summary>
        /// Deletes all asset conditons associated with layer.
        /// </summary>
        /// <param name="layerId">The id of the layer that contains asset condition.</param>
        /// <returns></returns>
        Task DeleteAsync(string layerId);

        /// <summary>
        /// Deletes an asset conditons.
        /// </summary>
        /// <param name="layerId">The id of the layer that contains asset condition.</param>
        /// <param name="asset">The id of asset associated with asset condition.</param>
        /// <returns></returns>
        Task DeleteAsync(string layerId, string asset);
    }
}
