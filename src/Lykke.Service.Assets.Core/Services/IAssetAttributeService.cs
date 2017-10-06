using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetAttributeService
    {
        Task AddAsync(string assetId, IAssetAttribute attribute);

        Task AddAsync(string assetId, string key, string value);

        Task<IAssetAttribute> GetAsync(string assetId, string key);

        Task<IEnumerable<IAssetAttributes>> GetAllAsync();

        Task<IEnumerable<IAssetAttributes>> GetAllAsync(string assetId);

        Task RemoveAsync(string assetId, string key);

        Task UpdateAsync(string assetId, IAssetAttribute attribute);

        Task UpdateAsync(string assetId, string key, string value);
    }
}