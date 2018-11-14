using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Responses.V2;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Cache
{
    public interface ICachedErc20TokenService
    {
        Task<Erc20Token> AddAsync(IErc20Token erc20Token);

        Task<IEnumerable<Erc20Token>> GetByAssetIdsAsync(string[] assetIds);

        Task<Erc20Token> GetByAssetIdAsync(string assetId);

        Task UpdateAsync(IErc20Token erc20Token);

        Task<Erc20Token> GetByTokenAddressAsync(string tokenAddress);

        Task<IEnumerable<Erc20Token>> GetAllWithAssetsAsync(string[] assetIds = null);
    }
}
