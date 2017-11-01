using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetGroupRepository
    {
        Task AddAsync(IAssetGroup group);

        Task<IEnumerable<IAssetGroup>> GetAllAsync();

        Task<IAssetGroup> GetAsync(string groupName);
        
        Task RemoveAsync(string groupName);
        
        Task UpdateAsync(IAssetGroup group);
    }
}