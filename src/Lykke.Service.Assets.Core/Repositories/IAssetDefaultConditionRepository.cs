using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetDefaultConditionRepository
    {
        /// <summary>
        /// Returns default asset conditions.
        /// </summary>
        /// <returns>The default asset conditions.</returns>
        Task<IAssetDefaultCondition> GetAsync(string layerId);

        /// <summary>
        /// Inserts a default asset conditions.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="assetDefaultCondition">The default asset conditons.</param>
        Task InsertAsync(string layerId, IAssetDefaultCondition assetDefaultCondition);

        /// <summary>
        /// Updates default asset conditions.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="assetDefaultCondition">The default asset conditions.</param>
        Task UpdateAsync(string layerId, IAssetDefaultCondition assetDefaultCondition);

        /// <summary>
        /// Deletes default asset conditons associated with layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        Task DeleteAsync(string layerId);
    }
}
