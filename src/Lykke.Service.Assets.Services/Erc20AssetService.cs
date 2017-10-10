using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class Erc20AssetService : IErc20AssetService
    {
        private readonly IErc20AssetRepository _erc20AssetRepository;

        public Erc20AssetService(
            IErc20AssetRepository erc20AssetRepository)
        {
            _erc20AssetRepository = erc20AssetRepository;
        }

        public async Task AddAsync(IErc20Asset asset)
        {
            await _erc20AssetRepository.SaveAsync(asset);
        }

        public async Task<IEnumerable<IErc20Asset>> GetAllAsync()
        {
            return await _erc20AssetRepository.GetAllAsync();
        }

        public async Task<IEnumerable<IErc20Asset>> GetAsync(string[] ids)
        {
            return await _erc20AssetRepository.GetByAssetIdAsync(ids);
        }

        public async Task<IErc20Asset> GetAsync(string id)
        {
            return await _erc20AssetRepository.GetByAssetIdAsync(id);
        }

        public async Task UpdateAsync(IErc20Asset asset)
        {
            await _erc20AssetRepository.SaveAsync(asset);
        }
    }
}