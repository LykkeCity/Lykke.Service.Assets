using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetRepository
    {
        Task AddAsync(IAsset asset);

        Task<IEnumerable<IAsset>> GetAllAsync();

        Task<IAsset> GetAsync(string id);

        Task<IEnumerable<IAsset>> GetAsync(string[] ids);

        Task RemoveAsync(string id);

        Task UpdateAsync(IAsset asset);
    }
}