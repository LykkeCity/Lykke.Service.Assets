using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IErc20AssetRepository
    {
        Task<IEnumerable<IErc20Asset>> GetAllAsync();

        Task<IErc20Asset> GetByAddressAsync(string tokenAddress);

        Task<IErc20Asset> GetByAssetIdAsync(string assetId);

        Task<IEnumerable<IErc20Asset>> GetByAssetIdAsync(string[] assetIds);

        Task SaveAsync(IErc20Asset erc20Asset);

        Task UpdateAsync(IErc20Asset erc20Asset);
    }
}
