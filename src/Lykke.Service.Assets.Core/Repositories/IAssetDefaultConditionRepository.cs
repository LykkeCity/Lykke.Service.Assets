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
        /// Adds or entirely replaces a default asset conditions.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="assetDefaultCondition">The default asset conditons.</param>
        Task InsertOrReplaceAsync(string layerId, IAssetDefaultCondition assetDefaultCondition);

        /// <summary>
        /// Deletes default asset conditons associated with layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        Task DeleteAsync(string layerId);
    }
}
