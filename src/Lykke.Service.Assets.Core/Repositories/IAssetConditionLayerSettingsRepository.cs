using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionLayerSettingsRepository
    {
        /// <summary>
        /// Returns default asset condition layer settings.
        /// </summary>
        /// <returns>The default asset condition layer settings.</returns>
        Task<IAssetConditionLayerSettings> GetAsync();

        /// <summary>
        /// Updates default asset conditions layer settings.
        /// </summary>
        /// <param name="settings">The asset conditon layer settings.</param>
        /// <returns></returns>
        Task UpdateAsync(IAssetConditionLayerSettings settings);
    }
}
