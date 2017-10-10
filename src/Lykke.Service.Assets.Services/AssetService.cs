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
    public class AssetService : IAssetService
    {
        private readonly IAssetRepository _assetRepository;


        public AssetService(
            IAssetRepository assetRepository)
        {
            _assetRepository = assetRepository;
        }


        public async Task AddAsync(IAsset asset)
        {
            await _assetRepository.AddAsync(asset);
        }

        public async Task Disable(string id)
        {
            var asset = Mapper.Map<Asset>(_assetRepository.GetAsync(id));

            asset.IsDisabled = true;

            await UpdateAsync(asset);
        }

        public async Task Enable(string id)
        {
            var asset = Mapper.Map<Asset>(_assetRepository.GetAsync(id));

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

        public async Task UpdateAsync(IAsset asset)
        {
            await _assetRepository.UpdateAsync(asset);
        }
    }
}