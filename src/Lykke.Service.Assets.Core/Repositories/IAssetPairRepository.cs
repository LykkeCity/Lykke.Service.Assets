using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetPairRepository
    {
        Task<IEnumerable<IAssetPair>> GetAllAsync();

        Task<IAssetPair> GetAsync(string id);

        Task UpsertAsync(IAssetPair assetPair);
    }
}