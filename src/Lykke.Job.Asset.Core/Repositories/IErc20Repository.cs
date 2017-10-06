using Lykke.Job.Asset.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Job.Asset.Core.Repositories
{
    public interface IErc20AssetRepository
    {
        Task<IEnumerable<IErc20Asset>> GetAllAsync();

        Task<IErc20Asset> GetByAddressAsync(string tokenAddress);

        Task<IErc20Asset> GetByAssetIdAsync(string assetId);

        Task SaveAsync(IErc20Asset erc20Asset);
    }
}
