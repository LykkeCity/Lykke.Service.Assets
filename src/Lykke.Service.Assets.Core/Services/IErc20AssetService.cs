using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;


namespace Lykke.Service.Assets.Core.Services
{
    public interface IErc20AssetService
    {
        Task AddAsync(IErc20Asset erc20Asset);

        Task<IEnumerable<IErc20Asset>> GetAllAsync();

        Task<IEnumerable<IErc20Asset>> GetAsync(string[] ids);

        Task<IErc20Asset> GetAsync(string id);

        Task UpdateAsync(IErc20Asset erc20Asset);
    }
}