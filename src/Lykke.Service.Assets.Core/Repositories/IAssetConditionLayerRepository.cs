using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionLayerRepository
    {
        /// <summary>
        /// Returns all asset conditions layers.
        /// </summary>
        /// <returns>A collection of conditions layers</returns>
        Task<IEnumerable<IAssetConditionLayer>> GetAsync();

        /// <summary>
        /// Returns asset conditions layer.
        /// </summary>
        /// <param name="layerId">The asset conditions layer id.</param>
        /// <returns>An asset conditions layer</returns>
        Task<IAssetConditionLayer> GetAsync(string layerId);

        /// <summary>
        /// Returns asset conditions layers.
        /// </summary>
        /// <param name="layerIds">The collection of layers id.</param>
        /// <returns>A collection of asset conditions layers</returns>
        Task<IEnumerable<IAssetConditionLayer>> GetAsync(IEnumerable<string> layerIds);

        /// <summary>
        /// Inserts a new entity if it does not exist, otherwise updates existing entity.
        /// </summary>
        /// <param name="layer">The asset conditon layer.</param>
        /// <returns></returns>
        Task InsertOrUpdateAsync(IAssetConditionLayer layer);

        /// <summary>
        /// Deletes an asset conditon layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <returns></returns>
        Task DeleteAsync(string layerId);
    }
}
