using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Responses.V2;

namespace Lykke.Service.Assets.Cache
{
    public interface ICachedAssetCategoryService
    {
        Task<AssetCategory> AddAsync(IAssetCategory assetCategory);

        Task<AssetCategory> GetAsync(string id);

        Task<IEnumerable<AssetCategory>> GetAllAsync();

        Task RemoveAsync(string id);

        Task UpdateAsync(IAssetCategory assetCategory);
    }
}
