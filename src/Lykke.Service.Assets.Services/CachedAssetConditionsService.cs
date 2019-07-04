using System;
using System.Collections.Generic;
using System.Linq;
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
        private readonly IAssetsForClientCacheManager _cacheManager;

        public CachedAssetConditionsService(
            IAssetDefaultConditionLayerRepository assetDefaultConditionLayerRepository,
            IAssetConditionRepository assetConditionRepository,
            IAssetDefaultConditionRepository assetDefaultConditionRepository,
            IDistributedCache<IAssetDefaultConditionLayer, AssetDefaultConditionLayerDto> assetDefaultConditionLayerCache,
            IDistributedCache<IAssetCondition, AssetConditionDto> assetConditionCache,
            IDistributedCache<IAssetDefaultCondition, AssetDefaultConditionDto> assetDefaultConditionCache,
            IAssetsForClientCacheManager cacheManager
            )
        {
            _assetDefaultConditionLayerRepository = assetDefaultConditionLayerRepository;
            _assetConditionRepository = assetConditionRepository;
            _assetDefaultConditionRepository = assetDefaultConditionRepository;

            #region Ondemand caches initialization
            _assetDefaultConditionLayerCache = assetDefaultConditionLayerCache;
            _assetConditionCache = assetConditionCache;
            _assetDefaultConditionCache = assetDefaultConditionCache;
            _cacheManager = cacheManager;

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

        public async Task AddAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _assetConditionRepository.InsertOrReplaceAsync(layerId, assetCondition);
            await _cacheManager.ClearCacheAsync("Added asset condition");

            var list = (await GetConditionsAsync(layerId)).ToList();

            var item = list.FirstOrDefault(x => x.Asset == assetCondition.Asset);

            if (item != null)
            {
                list.Remove(item);
            }

            list.Add(new AssetConditionDto
            {
                Asset = assetCondition.Asset,
                Regulation = assetCondition.Regulation,
                AvailableToClient = assetCondition.AvailableToClient,
                IsTradable = assetCondition.IsTradable,
                BankCardsDepositEnabled = assetCondition.BankCardsDepositEnabled,
                SwiftDepositEnabled = assetCondition.SwiftDepositEnabled
            });

            await _assetConditionCache.CacheDataAsync($"{layerId}", list);
        }

        public async Task DeleteAssetConditionAsync(string layerId, string assetId)
        {
            await _assetConditionRepository.DeleteAsync(layerId, assetId);
            await _cacheManager.ClearCacheAsync("Deleted asset condition");

            var list = (await GetConditionsAsync(layerId)).ToList();

            var item = list.FirstOrDefault(x => x.Asset == assetId);

            if (item != null)
            {
                list.Remove(item);
                await _assetConditionCache.CacheDataAsync($"{layerId}", list);
            }
        }

        public async Task DeleteAssetConditionsAsync(string layerId)
        {
            await _assetConditionRepository.DeleteAsync(layerId);
            await _cacheManager.ClearCacheAsync("Deleted asset condition");
            await _assetConditionCache.RemoveAsync(layerId);
        }
    }
}
