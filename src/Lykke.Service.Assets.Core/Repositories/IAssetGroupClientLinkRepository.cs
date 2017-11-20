using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetGroupClientLinkRepository
    {
        Task AddAsync(IAssetGroupClientLink groupClientLink);

        Task AddOrReplaceAsync(IAssetGroupClientLink groupClientLink);

        Task<IAssetGroupClientLink> GetAsync(string clientId, string groupName);

        Task<IEnumerable<IAssetGroupClientLink>> GetAllAsync();

        Task<IEnumerable<IAssetGroupClientLink>> GetAllAsync(string clientId);

        Task RemoveAsync(string clientId, string groupName);

        Task UpdateAsync(string clientId, IAssetGroup group);
    }
}
