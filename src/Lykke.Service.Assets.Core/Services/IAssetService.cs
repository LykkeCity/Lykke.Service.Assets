using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;


namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetService
    {
        Task<IAsset> AddAsync(IAsset asset);

        Task DisableAsync(string id);

        Task EnableAsync(string id);

        Task<IEnumerable<IAsset>> GetAllAsync();

        Task<IEnumerable<IAsset>> GetAsync(string[] ids);

        Task<IAsset> GetAsync(string id);

        Task<IEnumerable<IAsset>> GetForCategoryAsync(string categoryId);

        Task RemoveAsync(string id);

        Task UpdateAsync(IAsset asset);
    }
}