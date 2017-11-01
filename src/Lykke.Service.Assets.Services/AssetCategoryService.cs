using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class AssetCategoryService : IAssetCategoryService
    {
        private readonly IAssetCategoryRepository _assetCategoryRepository;


        public AssetCategoryService(
            IAssetCategoryRepository assetCategoryRepository)
        {
            _assetCategoryRepository = assetCategoryRepository;
        }


        public async Task<IAssetCategory> AddAsync(IAssetCategory assetCategory)
        {
            await _assetCategoryRepository.AddAsync(assetCategory);

            return assetCategory;
        }

        public async Task<IAssetCategory> GetAsync(string id)
        {
            return await _assetCategoryRepository.GetAsync(id);
        }

        public async Task<IEnumerable<IAssetCategory>> GetAllAsync()
        {
            return await _assetCategoryRepository.GetAllAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await _assetCategoryRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAssetCategory assetCategory)
        {
            await _assetCategoryRepository.UpdateAsync(assetCategory);
        }
    }
}