using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionLayerRepository
    {
        Task<IReadOnlyList<IAssetConditionLayer>> GetByIdsAsync(IEnumerable<string> layerIds);
    }
}
