using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class MarginAssetPairService : IMarginAssetPairService
    {
        private readonly IMarginAssetPairRepository _marginAssetPairRepository;


        public MarginAssetPairService(
            IMarginAssetPairRepository marginAssetPairRepository)
        {
            _marginAssetPairRepository = marginAssetPairRepository;
        }


        public async Task<IMarginAssetPair> AddAsync(IMarginAssetPair marginAssetPair)
        {
            await _marginAssetPairRepository.AddAsync(marginAssetPair);

            return marginAssetPair;
        }

        public IMarginAssetPair CreateDefault()
        {
            return new MarginAssetPair
            {
                Accuracy = 5,
            };
        }

        public async Task<IEnumerable<IMarginAssetPair>> GetAllAsync()
        {
            return await _marginAssetPairRepository.GetAllAsync();
        }

        public async Task<IMarginAssetPair> GetAsync(string id)
        {
            return await _marginAssetPairRepository.GetAsync(id);
        }

        public async Task RemoveAsync(string id)
        {
            await _marginAssetPairRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(IMarginAssetPair marginAssetPair)
        {
            await _marginAssetPairRepository.UpdateAsync(marginAssetPair);
        }
    }
}