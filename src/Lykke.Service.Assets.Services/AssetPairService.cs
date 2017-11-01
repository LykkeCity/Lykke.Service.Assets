using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetPairService : IAssetPairService
    {
        private readonly IAssetPairRepository _assetPairRepository;


        public AssetPairService(
            IAssetPairRepository assetPairRepository)
        {
            _assetPairRepository = assetPairRepository;
        }


        public async Task<IAssetPair> AddAsync(IAssetPair assetPair)
        {
            await _assetPairRepository.UpsertAsync(assetPair);

            return assetPair;
        }

        public IAssetPair CreateDefault()
        {
            return new AssetPair
            {
                Accuracy   = 5,
                IsDisabled = false
            };
        }

        public async Task<IEnumerable<IAssetPair>> GetAllAsync()
        {
            return await _assetPairRepository.GetAllAsync();
        }

        public async Task<IAssetPair> GetAsync(string id)
        {
            return await _assetPairRepository.GetAsync(id);
        }

        public async Task RemoveAsync(string id)
        {
            await _assetPairRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAssetPair assetPair)
        {
            await _assetPairRepository.UpsertAsync(assetPair);
        }
    }
}