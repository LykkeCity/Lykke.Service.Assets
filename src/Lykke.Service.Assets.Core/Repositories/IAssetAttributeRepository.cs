using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetAttributeRepository
    {
        Task AddAsync(string assetId, IAssetAttribute attribute);
        
        Task<IAssetAttribute> GetAsync(string assetId, string key);

        Task<IEnumerable<(string AssetId, IAssetAttribute Attribute)>> GetAllAsync();

        Task<IEnumerable<IAssetAttribute>> GetAllAsync(string assetId);

        Task RemoveAsync(string assetId, string key);

        Task UpdateAsync(string assetId, IAssetAttribute attribute);
    }
}