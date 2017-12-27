using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetConditionSettingsService : IAssetConditionSettingsService
    {
        private readonly IAssetConditionSettingsRepository _assetConditionSettingsRepository;
        private readonly IAssetConditionLayerSettingsRepository _assetConditionLayerSettingsRepository;
        private readonly IAssetsForClientCacheManager _cacheManager;

        public AssetConditionSettingsService(
            IAssetConditionSettingsRepository assetConditionSettingsRepository,
            IAssetConditionLayerSettingsRepository assetConditionLayerSettingsRepository,
            IAssetsForClientCacheManager cacheManager)
        {
            _assetConditionSettingsRepository = assetConditionSettingsRepository;
            _assetConditionLayerSettingsRepository = assetConditionLayerSettingsRepository;
            _cacheManager = cacheManager;
        }

        public async Task<IAssetConditionLayerSettings> GetConditionLayerSettingsAsync()
        {
            IAssetConditionLayerSettings settings = await _assetConditionLayerSettingsRepository.GetAsync();

            return Mapper.Map<AssetConditionLayerSettings>(settings);
        }

        public async Task<IAssetConditionSettings> GetConditionSettingsAsync()
        {
            IAssetConditionSettings settings = await _assetConditionSettingsRepository.GetAsync();

            return Mapper.Map<AssetConditionSettings>(settings);
        }

        public async Task UpdateAsync(IAssetConditionLayerSettings layerSettings, IAssetConditionSettings assetSettings)
        {
            Task assetSettingsTask = _assetConditionSettingsRepository.UpdateAsync(assetSettings);
            Task layerSettingsTask = _assetConditionLayerSettingsRepository.UpdateAsync(layerSettings);

            await Task.WhenAll(assetSettingsTask, layerSettingsTask);

            await _cacheManager.ClearCacheAsync("Default asset conditions settings changed");
        }
    }
}
