using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetDefaultConditionLayerRepository
    {
        /// <summary>
        /// Returns default asset condition layer.
        /// </summary>
        /// <returns>The default asset condition layer.</returns>
        Task<IAssetDefaultConditionLayer> GetAsync();

        /// <summary>
        /// Updates default asset conditions layer.
        /// </summary>
        /// <param name="settings">The asset conditions layer settings.</param>
        Task UpdateAsync(IAssetConditionLayerSettings settings);
    }
}
