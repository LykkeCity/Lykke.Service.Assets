using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IMarginAssetRepository
    {
        Task AddAsync(IMarginAsset asset);

        Task UpdateAsync(IMarginAsset asset);

        Task<IEnumerable<IMarginAsset>> GetAllAsync();

        Task<IMarginAsset> GetAsync(string id);
    }
}