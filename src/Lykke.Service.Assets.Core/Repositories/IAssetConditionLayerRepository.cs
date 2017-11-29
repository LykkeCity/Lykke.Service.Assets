using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionLayerRepository
    {
        Task<IReadOnlyList<IAssetConditionLayer>> GetByIdsAsync(IEnumerable<string> layerIds);
        Task<IReadOnlyList<IAssetConditionLayer>> GetLayerListAsync();
        Task InsertOrUpdateAssetConditionsToLayer(string layerId, IAssetConditions assetConditions);
        Task InsetLayerAsync(IAssetConditionLayer layer);
        Task UpdateLayerAsync(IAssetConditionLayer layer);
        Task DeleteLayerAsync(string layerId);
    }
}
