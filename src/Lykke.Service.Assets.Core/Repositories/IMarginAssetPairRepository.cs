using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IMarginAssetPairRepository
    {
        Task AddAsync(IMarginAssetPair marginAssetPair);

        Task<IEnumerable<IMarginAssetPair>> GetAllAsync();

        Task<IMarginAssetPair> GetAsync(string id);

        Task UpdateAsync(IMarginAssetPair marginAssetPair);
    }
}