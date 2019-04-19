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
    }
}
