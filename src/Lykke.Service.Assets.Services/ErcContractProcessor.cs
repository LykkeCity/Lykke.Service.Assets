using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Services
{
    public class ErcContractProcessor : IErcContractProcessor
    {
        private readonly IErc20TokenRepository _erc20TokenRepository;

        public ErcContractProcessor(IErc20TokenRepository erc20TokenRepository)
        {
            _erc20TokenRepository = erc20TokenRepository;
        }

        //TODO: Add more logic here
        public async Task ProcessErc20ContractAsync(IErc20Token message)
        {
            var existingContract = await _erc20TokenRepository.GetByAddressAsync(message.Address);

            if (existingContract != null)
            {
                await _erc20TokenRepository.AddAsync(message);
            }
        }
    }
}
