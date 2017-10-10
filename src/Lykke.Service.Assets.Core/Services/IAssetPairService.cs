using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetPairService
    {
        Task<IAssetPair> AddAsync(IAssetPair assetPair);

        IAssetPair CreateDefault();

        Task<IEnumerable<IAssetPair>> GetAllAsync();

        Task<IAssetPair> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(IAssetPair assetPair);
    }
}