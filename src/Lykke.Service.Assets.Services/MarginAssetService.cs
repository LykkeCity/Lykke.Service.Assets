using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class MarginAssetService : IMarginAssetService
    {
        private readonly IMarginAssetRepository _marginAssetRepository;


        public MarginAssetService(
            IMarginAssetRepository marginAssetRepository)
        {
            _marginAssetRepository = marginAssetRepository;
        }


        public async Task AddAsync(IMarginAsset marginAsset)
        {
            await _marginAssetRepository.AddAsync(marginAsset);
        }

        public IMarginAsset CreateDefault()
        {
            return new MarginAsset();
        }

        public async Task<IEnumerable<IMarginAsset>> GetAllAsync()
        {
            return await _marginAssetRepository.GetAllAsync();
        }

        public async Task<IMarginAsset> GetAsync(string id)
        {
            return await _marginAssetRepository.GetAsync(id);
        }

        public async Task UpdateAsync(IMarginAsset marginAsset)
        {
            await _marginAssetRepository.UpdateAsync(marginAsset);
        }
    }
}