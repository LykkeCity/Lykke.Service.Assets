using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class Erc20TokenService : IErc20TokenService
    {
        private readonly IErc20TokenRepository _erc20TokenRepository;

        public Erc20TokenService(
            IErc20TokenRepository erc20TokenRepository)
        {
            _erc20TokenRepository = erc20TokenRepository;
        }

        public async Task<IErc20Token> AddAsync(IErc20Token token)
        {
            await _erc20TokenRepository.AddAsync(token);

            return token;
        }

        public async Task<IEnumerable<IErc20Token>> GetAllAsync()
        {
            return await _erc20TokenRepository.GetAllAsync();
        }

        public async Task<IEnumerable<IErc20Token>> GetAsync(string[] ids)
        {
            return await _erc20TokenRepository.GetByAssetIdAsync(ids);
        }

        public async Task<IErc20Token> GetAsync(string id)
        {
            return await _erc20TokenRepository.GetByAssetIdAsync(id);
        }

        public async Task UpdateAsync(IErc20Token token)
        {
            await _erc20TokenRepository.AddAsync(token);
        }

        public async Task<IErc20Token> GetByTokenAddressAsync(string address)
        {
            return await _erc20TokenRepository.GetByAddressAsync(address);
        }

        public async Task<IEnumerable<IErc20Token>> GetAllWithAssetsAsync()
        {
            return await _erc20TokenRepository.GetAllWithAssetsAsync();
        }
    }
}