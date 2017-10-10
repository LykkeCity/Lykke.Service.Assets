using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;


namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetService
    {
        Task AddAsync(IAsset asset);

        Task Disable(string id);

        Task Enable(string id);

        Task<IEnumerable<IAsset>> GetAllAsync();

        Task<IEnumerable<IAsset>> GetAsync(string[] ids);

        Task<IAsset> GetAsync(string id);

        Task<IEnumerable<IAsset>> GetForCategoryAsync(string categoryId);

        Task UpdateAsync(IAsset asset);
    }
}