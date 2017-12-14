using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class AssetConditionDefaultLayerService : IAssetConditionDefaultLayerService
    {
        private readonly IAssetConditionDefaultLayerRepository _assetConditionDefaultLayerRepository;
        private readonly IAssetsForClientCacheManager _cacheManager;

        public AssetConditionDefaultLayerService(
            IAssetConditionDefaultLayerRepository assetConditionDefaultLayerRepository,
            IAssetsForClientCacheManager cacheManager)
        {
            _assetConditionDefaultLayerRepository = assetConditionDefaultLayerRepository;
            _cacheManager = cacheManager;
        }

        public async Task<IAssetConditionDefaultLayer> GetAsync()
        {
            return await _assetConditionDefaultLayerRepository.GetAsync();
        }

        public async Task InsertOrUpdateAsync(IAssetConditionDefaultLayer settings)
        {
            await _cacheManager.ClearCache("Default layer changed");
            await _assetConditionDefaultLayerRepository.InsertOrUpdate(settings);
        }
    }
}
