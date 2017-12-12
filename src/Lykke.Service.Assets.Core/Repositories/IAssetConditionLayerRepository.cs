using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionLayerRepository
    {
        Task<IReadOnlyList<IAssetConditionLayer>> GetByIdsAsync(IEnumerable<string> layerIds);
        Task<IReadOnlyList<IAssetConditionLayer>> GetLayerListAsync();
        Task InsertOrUpdateAssetConditionsToLayerAsync(string layerId, IAssetCondition assetCondition);
        Task DeleteAssetConditionsAsync(string layerId, string asset);
        Task InsetLayerAsync(IAssetConditionLayer layer);
        Task UpdateLayerAsync(IAssetConditionLayer layer);
        Task DeleteLayerAsync(string layerId);
    }
}
