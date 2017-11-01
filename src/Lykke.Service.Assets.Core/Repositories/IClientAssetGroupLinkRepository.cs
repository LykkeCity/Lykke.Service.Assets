using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IClientAssetGroupLinkRepository
    {
        Task AddAsync(IClientAssetGroupLink clientGroupLink);

        Task<IClientAssetGroupLink> GetAsync(string clientId, string groupName);

        Task<IEnumerable<IClientAssetGroupLink>> GetAllAsync();

        Task<IEnumerable<IClientAssetGroupLink>> GetAllAsync(string groupName);

        Task RemoveAsync(string clientId, string groupName);

        Task UpdateAsync(string clientId, IAssetGroup group);
    }
}