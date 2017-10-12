using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IMarginAssetRepository
    {
        Task AddAsync(IMarginAsset marginAsset);
        
        Task<IEnumerable<IMarginAsset>> GetAllAsync();

        Task<IMarginAsset> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(IMarginAsset marginAsset);
    }
}