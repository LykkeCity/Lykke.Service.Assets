using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Common.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Microsoft.Extensions.Caching.Memory;

namespace Lykke.Service.Assets.Services
{
    public class CachedAssetConditionsService : ICachedAssetConditionsService
    {
        #region Repositories
        private readonly IAssetDefaultConditionLayerRepository _assetDefaultConditionLayerRepository;
        private readonly IAssetConditionRepository _assetConditionRepository;
        private readonly IAssetDefaultConditionRepository _assetDefaultConditionRepository;
        #endregion
        
        #region Ondemand caches
        private readonly OnDemandDataCache<IAssetDefaultConditionLayer> _assetDefaultConditionLayerCache;
        private readonly OnDemandDataCache<IEnumerable<IAssetCondition>> _assetConditionCache;
        private readonly OnDemandDataCache<IAssetConditionSettings> _assetDefaultConditionCache;
        #endregion
        
        private readonly TimeSpan _cacheExpirationPeriod;

        public CachedAssetConditionsService(
            IMemoryCache memoryCache, 
            TimeSpan cacheExpirationPeriod, 
            IAssetDefaultConditionLayerRepository assetDefaultConditionLayerRepository, 
            IAssetConditionRepository assetConditionRepository, 
            IAssetDefaultConditionRepository assetDefaultConditionRepository)
        {
            _cacheExpirationPeriod = cacheExpirationPeriod;
            _assetDefaultConditionLayerRepository = assetDefaultConditionLayerRepository;
            _assetConditionRepository = assetConditionRepository;
            _assetDefaultConditionRepository = assetDefaultConditionRepository;
            
            #region Ondemand caches initialization
            _assetDefaultConditionLayerCache = new OnDemandDataCache<IAssetDefaultConditionLayer>(memoryCache);
            _assetConditionCache = new OnDemandDataCache<IEnumerable<IAssetCondition>>(memoryCache);
            _assetDefaultConditionCache = new OnDemandDataCache<IAssetConditionSettings>(memoryCache);
            #endregion
        }

        public Task<IEnumerable<IAssetCondition>> GetConditions(string layerId)
        {
            if (string.IsNullOrEmpty(layerId))
                throw new ArgumentNullException(nameof(layerId));
            
            return _assetConditionCache.GetOrAddAsync(
                $"assetConditionCache-{layerId}", 
                async x => await _assetConditionRepository.GetAsync(layerId), 
                GetExpirationDate());
        }

        public Task<IAssetConditionSettings> GetDefaultConditions(string layerId)
        {
            if (string.IsNullOrEmpty(layerId))
                throw new ArgumentNullException(nameof(layerId));
            
            return _assetDefaultConditionCache.GetOrAddAsync(
                $"assetDefaultConditionCache-{layerId}",
                async x => await _assetDefaultConditionRepository.GetAsync(layerId),
                GetExpirationDate());
        }

        public Task<IAssetDefaultConditionLayer> GetDefaultLayer()
        {
            return _assetDefaultConditionLayerCache.GetOrAddAsync(
                "assetDefaultLayerCache", 
                async x => await _assetDefaultConditionLayerRepository.GetAsync(),
                GetExpirationDate());
        }
        
        private DateTime GetExpirationDate() 
            => DateTime.UtcNow.Add(_cacheExpirationPeriod);
    }
}
