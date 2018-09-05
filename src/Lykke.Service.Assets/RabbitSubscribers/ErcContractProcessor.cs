using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;

namespace Lykke.Service.Assets.Services
{
    public class ErcContractProcessor : IErcContractProcessor
    {
        private readonly ICachedErc20TokenService _erc20TokenService;

        public ErcContractProcessor(ICachedErc20TokenService erc20TokenService)
        {
            _erc20TokenService = erc20TokenService;
        }

        public async Task ProcessErc20ContractAsync(IErc20Token token)
        {
            var existingContract = await _erc20TokenService.GetByTokenAddressAsync(token.Address);

            if (existingContract == null)
            {
                await _erc20TokenService.AddAsync(token);
            }
        }
    }
}
