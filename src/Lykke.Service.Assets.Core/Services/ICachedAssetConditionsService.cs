using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface ICachedAssetConditionsService
    {
        Task<IEnumerable<IAssetCondition>> GetConditionsAsync(string layerId);
        Task<IAssetConditionSettings> GetDefaultConditionsAsync(string layerId);
        Task<IAssetDefaultConditionLayer> GetDefaultLayerAsync();
        Task AddAssetConditionAsync(string layerId, IAssetCondition assetCondition);
        Task DeleteAssetConditionAsync(string layerId, string assetId);
        Task DeleteAssetConditionsAsync(string layerId);
    }
}
