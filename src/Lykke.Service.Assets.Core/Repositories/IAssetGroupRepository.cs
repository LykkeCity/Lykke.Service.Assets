using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetGroupRepository
    {
        Task<IEnumerable<(AssetGroupType Type, IAssetGroup AssetGroup)>> GetAllAsync();

        Task<IEnumerable<IAssetGroup>> GetAllAsync(AssetGroupType type);

        Task<IEnumerable<IAssetGroup>> GetAllAsync(AssetGroupType type, string id);
    }
}