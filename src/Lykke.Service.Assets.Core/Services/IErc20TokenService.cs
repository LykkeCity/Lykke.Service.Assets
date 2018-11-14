using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IErc20TokenService
    {
        Task<IErc20Token> AddAsync(IErc20Token erc20Token);

        Task<IErc20Token> GetByAssetIdAsync(string assetId);

        Task UpdateAsync(IErc20Token erc20Token);

        Task<IErc20Token> GetByTokenAddressAsync(string tokenAddress);

        Task<IEnumerable<IErc20Token>> GetAllWithAssetsAsync(IEnumerable<string> assetIds);
    }
}
