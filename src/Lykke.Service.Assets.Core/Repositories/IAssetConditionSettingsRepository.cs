using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionSettingsRepository
    {
        /// <summary>
        /// Returns default asset conditions settings.
        /// </summary>
        /// <returns>The default asset conditions settings.</returns>
        Task<IAssetConditionSettings> GetAsync();

        /// <summary>
        /// Updates default asset conditions settings.
        /// </summary>
        /// <param name="settings">The default asset conditions settings.</param>
        /// <returns></returns>
        Task UpdateAsync(IAssetConditionSettings settings);
    }
}
