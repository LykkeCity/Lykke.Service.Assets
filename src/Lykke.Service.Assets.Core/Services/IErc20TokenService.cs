using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;


namespace Lykke.Service.Assets.Core.Services
{
    public interface IErc20TokenService
    {
        Task<IErc20Token> AddAsync(IErc20Token erc20Token);

        Task<IEnumerable<IErc20Token>> GetAllAsync();

        Task<IEnumerable<IErc20Token>> GetAsync(string[] ids);

        Task<IErc20Token> GetAsync(string id);

        Task UpdateAsync(IErc20Token erc20Token);
    }
}