using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;


        public AssetService(
            IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }


        public async Task<IAsset> AddAsync(IAsset asset)
        {
            await ValidateAsset(asset);

            await _assetRepository.AddAsync(asset);

            return asset;
        }

        public IAsset CreateDefault()
        {
            return new Asset();
        }

        public async Task DisableAsync(string id)
        {
            var asset = Mapper.Map<Asset>(await _assetRepository.GetAsync(id));

            asset.IsDisabled = true;

            await UpdateAsync(asset);
        }

        public async Task EnableAsync(string id)
        {
            var asset = Mapper.Map<Asset>(await _assetRepository.GetAsync(id));

            asset.IsDisabled = false;

            await UpdateAsync(asset);
        }

        public async Task<IEnumerable<IAsset>> GetAllAsync()
        {
            return await _assetRepository.GetAllAsync();
        }

        public async Task<IEnumerable<IAsset>> GetAsync(string[] ids)
        {
            return await _assetRepository.GetAsync(ids);
        }

        public async Task<IAsset> GetAsync(string id)
        {
            return await _assetRepository.GetAsync(id);
        }

        public async Task<IEnumerable<IAsset>> GetForCategoryAsync(string categoryId)
        {
            var assets = await _assetRepository.GetAllAsync();

            return assets.Where(x => x.CategoryId == categoryId);
        }

        public async Task RemoveAsync(string id)
        {
            await _assetRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAsset asset)
        {
            await ValidateAsset(asset);

            await _assetRepository.UpdateAsync(asset);
        }

        private async Task ValidateAsset(IAsset asset)
        {
            ValidateAccuracyAndMultiplierPower(asset);

            await ValidateBlockchainAssetId(asset);
        }

        private static void ValidateAccuracyAndMultiplierPower(IAsset asset)
        {
            if (asset.Accuracy > asset.MultiplierPower)
            {
                throw new ValidationException($"Asset accuracy [{asset.Accuracy}] should be less or equal to multiplier power [{asset.MultiplierPower}].");
            }
        }
        
        private async Task ValidateBlockchainAssetId(IAsset asset)
        {
            if (!string.IsNullOrEmpty(asset.BlockChainAssetId))
            {
                var assets = await _assetRepository.GetAllAsync();

                if (assets.Any(x => x.BlockChainAssetId == asset.BlockChainAssetId && x.Id != asset.Id))
                {
                    throw new ValidationException($"Another asset [{asset.Id}] with specified BlockChainAssetId [{asset.BlockChainAssetId}] already exists");
                }
            }
        }
    }
}