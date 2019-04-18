using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface ICachedAssetConditionsService
    {
        Task<IEnumerable<IAssetCondition>> GetConditions(string layerId);
        Task<IAssetConditionSettings> GetDefaultConditions(string layerId);
        Task<IAssetDefaultConditionLayer> GetDefaultLayer();
    }
}
