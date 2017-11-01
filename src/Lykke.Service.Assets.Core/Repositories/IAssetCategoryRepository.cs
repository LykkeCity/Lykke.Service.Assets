using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;


namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetCategoryRepository
    {
        Task AddAsync(IAssetCategory assetCategory);

        Task<IAssetCategory> GetAsync(string id);

        Task<IEnumerable<IAssetCategory>> GetAllAsync();

        Task RemoveAsync(string id);

        Task UpdateAsync(IAssetCategory assetCategory);
    }
}