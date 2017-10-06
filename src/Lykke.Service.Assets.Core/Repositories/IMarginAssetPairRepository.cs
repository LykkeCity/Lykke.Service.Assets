using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IMarginAssetPairRepository
    {
        Task<IEnumerable<IMarginAssetPair>> GetAllAsync();

        Task<IMarginAssetPair> GetAsync(string id);

        Task AddAsync(IMarginAssetPair assetPair);

        Task EditAsync(string id, IMarginAssetPair assetPair);
    }
}