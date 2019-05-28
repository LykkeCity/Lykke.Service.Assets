using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.DTOs;

namespace Lykke.Service.Assets.Services
{
    public class CachedAssetConditionsService : ICachedAssetConditionsService
    {
        private readonly IAssetDefaultConditionLayerRepository _assetDefaultConditionLayerRepository;
        private readonly IAssetConditionRepository _assetConditionRepository;
        private readonly IAssetDefaultConditionRepository _assetDefaultConditionRepository;
        private readonly IDistributedCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto> _assetDefaultConditionLayerCache;
        private readonly IDistributedCache<IAssetCondition, AssetConditionDto> _assetConditionCache;
        private readonly IDistributedCache<IAssetDefaultCondition, AssetDefaultConditionDto> _assetDefaultConditionCache;

        public CachedAssetConditionsService(
            IAssetDefaultConditionLayerRepository assetDefaultConditionLayerRepository, 
            IAssetConditionRepository assetConditionRepository, 
            IAssetDefaultConditionRepository assetDefaultConditionRepository,
            IDistributedCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto> assetDefaultConditionLayerCache,
            IDistributedCache<IAssetCondition, AssetConditionDto> assetConditionCache,
            IDistributedCache<IAssetDefaultCondition, AssetDefaultConditionDto> assetDefaultConditionCache
            )
        {
            _assetDefaultConditionLayerRepository = assetDefaultConditionLayerRepository;
            _assetConditionRepository = assetConditionRepository;
            _assetDefaultConditionRepository = assetDefaultConditionRepository;
            
            #region Ondemand caches initialization
            _assetDefaultConditionLayerCache = assetDefaultConditionLayerCache;
            _assetConditionCache = assetConditionCache;
            _assetDefaultConditionCache = assetDefaultConditionCache;
            #endregion
        }

        public async Task<IEnumerable<IAssetCondition>> GetConditionsAsync(string layerId)
        {
            if (string.IsNullOrEmpty(layerId))
                throw new ArgumentNullException(nameof(layerId));
            
            return await _assetConditionCache.GetListAsync($"{layerId}", 
                async () => await _assetConditionRepository.GetAsync(layerId));
        }

        public async Task<IAssetConditionSettings> GetDefaultConditionsAsync(string layerId)
        {
            if (string.IsNullOrEmpty(layerId))
                throw new ArgumentNullException(nameof(layerId));
            
            return await _assetDefaultConditionCache.GetAsync($"{layerId}",
                async () => await _assetDefaultConditionRepository.GetAsync(layerId));
        }

        public async Task<IAssetDefaultConditionLayer> GetDefaultLayerAsync()
        {
            return await _assetDefaultConditionLayerCache.GetAsync("default", 
                async () => await _assetDefaultConditionLayerRepository.GetAsync());
        }
    }
}
