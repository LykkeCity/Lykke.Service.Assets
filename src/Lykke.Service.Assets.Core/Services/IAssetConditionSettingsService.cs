using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetConditionSettingsService
    {
        Task<IAssetConditionSettings> GetConditionSettingsAsync();

        Task<IAssetConditionLayerSettings> GetConditionLayerSettingsAsync();

        Task UpdateAsync(IAssetConditionLayerSettings layerSettings, IAssetConditionSettings assetSettings);
    }
}
