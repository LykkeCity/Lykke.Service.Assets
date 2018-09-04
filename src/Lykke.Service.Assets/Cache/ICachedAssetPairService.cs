using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Responses.V2;

namespace Lykke.Service.Assets.Cache
{
    public interface ICachedAssetPairService
    {
        Task<AssetPair> AddAsync(IAssetPair assetPair);

        AssetPair CreateDefault();

        Task<IEnumerable<AssetPair>> GetAllAsync();

        Task<AssetPair> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(IAssetPair assetPair);
    }
}
