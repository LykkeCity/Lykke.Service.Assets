using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetGroupAssetLinkRepository
    {
        Task AddAsync(IAssetGroupAssetLink assetLink);

        Task<IAssetGroupAssetLink> GetAsync(string assetId, string groupName);

        Task<IEnumerable<IAssetGroupAssetLink>> GetAllAsync();

        Task<IEnumerable<IAssetGroupAssetLink>> GetAllAsync(string groupName);

        Task RemoveAsync(string assetId, string groupName);

        Task UpdateAsync(string assetId, IAssetGroup group);
    }
}