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
        private readonly IErc20AssetRepository _erc20AssetRepository;

        public ErcContractProcessor(IErc20AssetRepository erc20AssetRepository)
        {
            _erc20AssetRepository = erc20AssetRepository;
        }

        //TODO: Add more logic here
        public async Task ProcessErc20ContractAsync(IErc20Asset message)
        {
            var existingContract = await _erc20AssetRepository.GetByAddressAsync(message.Address);

            if (existingContract != null)
            {
                await _erc20AssetRepository.SaveAsync(message);
            }
        }
    }
}
