﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetConditionService : IAssetConditionService
    {
        private readonly IAssetConditionRepository _assetConditionRepository;
        private readonly IAssetConditionLayerRepository _assetConditionLayerRepository;
        private readonly IAssetDefaultConditionRepository _assetDefaultConditionRepository;
        private readonly IAssetDefaultConditionLayerRepository _assetDefaultConditionLayerRepository;
        private readonly IAssetConditionLayerLinkClientRepository _assetConditionLayerLinkClientRepository;
        private readonly IAssetsForClientCacheManager _cacheManager;

        public AssetConditionService(
            IAssetConditionRepository assetConditionRepository, 
            IAssetConditionLayerRepository assetConditionLayerRepository,
            IAssetDefaultConditionRepository assetDefaultConditionRepository,
            IAssetDefaultConditionLayerRepository assetDefaultConditionLayerRepository,
            IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository,
            IAssetsForClientCacheManager cacheManager)
        {
            _assetConditionRepository = assetConditionRepository;
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetDefaultConditionRepository = assetDefaultConditionRepository;
            _assetDefaultConditionLayerRepository = assetDefaultConditionLayerRepository;
            _assetConditionLayerLinkClientRepository = assetConditionLayerLinkClientRepository;
            _cacheManager = cacheManager;
        }

        public async Task<IEnumerable<IAssetConditionLayer>> GetLayersAsync()
        {
            return await _assetConditionLayerRepository.GetAsync();
        }

        public async Task<IAssetConditionLayer> GetLayerAsync(string layerId)
        {
            IAssetConditionLayer layer = await _assetConditionLayerRepository.GetAsync(layerId);

            if (layer == null)
                return null;

            IEnumerable<IAssetCondition> conditions = await _assetConditionRepository.GetAsync(layer.Id);
            IAssetDefaultCondition defaultCondition = await _assetDefaultConditionRepository.GetAsync(layer.Id);

            var model = Mapper.Map<AssetConditionLayer>(layer);

            model.AssetConditions = conditions.ToList();
            model.AssetDefaultCondition = defaultCondition;

            return model;
        }

        public async Task<IAssetDefaultConditionLayer> GetDefaultLayerAsync()
        {
            IAssetDefaultConditionLayer defaultLayer = await _assetDefaultConditionLayerRepository.GetAsync();

            IEnumerable<IAssetCondition> conditions = await _assetConditionRepository.GetAsync(defaultLayer.Id);

            var model = Mapper.Map<AssetDefaultConditionLayer>(defaultLayer);

            model.AssetConditions = conditions.ToList();

            return model;
        }

        public async Task AddAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _assetConditionRepository.InsertOrReplaceAsync(layerId, assetCondition);

            await _cacheManager.ClearCacheAsync("Added asset condition");
        }

        public async Task UpdateAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _assetConditionRepository.InsertOrReplaceAsync(layerId, assetCondition);

            await _cacheManager.ClearCacheAsync("Updated asset condition");
        }

        public async Task DeleteAssetConditionAsync(string layerId, string assetId)
        {
            await _assetConditionRepository.DeleteAsync(layerId, assetId);

            await _cacheManager.ClearCacheAsync("Deleted asset condition");
        }

        public async Task AddDefaultAssetConditionAsync(string layerId, IAssetDefaultCondition assetDefaultCondition)
        {
            await _assetDefaultConditionRepository.InsertOrReplaceAsync(layerId, assetDefaultCondition);

            await _cacheManager.ClearCacheAsync("Added default asset condition");
        }

        public async Task UpdateDefaultAssetConditionAsync(string layerId, IAssetDefaultCondition assetDefaultCondition)
        {
            await _assetDefaultConditionRepository.InsertOrReplaceAsync(layerId, assetDefaultCondition);

            await _cacheManager.ClearCacheAsync("Updated default asset condition");
        }

        public async Task DeleteDefaultAssetConditionAsync(string layerId)
        {
            await _assetDefaultConditionRepository.DeleteAsync(layerId);

            await _cacheManager.ClearCacheAsync("Deleted default asset condition");
        }

        public async Task AddLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerRepository.InsertOrReplaceAsync(layer);
        }

        public async Task UpdateLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerRepository.InsertOrReplaceAsync(layer);

            await _cacheManager.ClearCacheAsync("Updated condition layer");
        }

        public async Task DeleteLayerAsync(string layerId)
        {
            await Task.WhenAll(
                _assetConditionLayerLinkClientRepository.RemoveLayerFromClientsAsync(layerId),
                _assetConditionRepository.DeleteAsync(layerId),
                _assetConditionLayerRepository.DeleteAsync(layerId),
                _assetDefaultConditionRepository.DeleteAsync(layerId));

            await _cacheManager.ClearCacheAsync("Deleted condition layer");
        }

        public async Task UpdateDefaultLayerAsync(IAssetConditionLayerSettings settings)
        {
            await _assetDefaultConditionLayerRepository.InsertOrReplaceAsync(settings);

            await _cacheManager.ClearCacheAsync("Default asset condition layer changed");
        }

        public async Task<IEnumerable<IAssetConditionLayer>> GetClientLayers(string clientId)
        {
            IEnumerable<IAssetConditionLayer> layers = await GetLayersAsync(clientId);

            var model = Mapper.Map<List<AssetConditionLayer>>(layers);

            foreach (AssetConditionLayer layer in model)
            {
                IEnumerable<IAssetCondition> conditions = await _assetConditionRepository.GetAsync(layer.Id);
                IAssetDefaultCondition defaultCondition = await _assetDefaultConditionRepository.GetAsync(layer.Id);

                layer.AssetConditions = conditions.ToList();
                layer.AssetDefaultCondition = defaultCondition;
            }

            return model;
        }

        public async Task AddClientLayerAsync(string clientId, string layerId)
        {
            await _assetConditionLayerLinkClientRepository.AddAsync(clientId, layerId);

            await _cacheManager.RemoveClientFromCacheAsync(clientId);
        }

        public async Task RemoveClientLayerAsync(string clientId, string layerId)
        {
            await _assetConditionLayerLinkClientRepository.RemoveAsync(clientId, layerId);

            await _cacheManager.RemoveClientFromCacheAsync(clientId);
        }

        public async Task<IEnumerable<IAssetCondition>> GetAssetConditionsByClient(string clientId)
        {
            IList<IAssetCondition> assetConditions = await _cacheManager.TryGetAssetConditionsForClientAsync(clientId);

            if (assetConditions != null)
                return assetConditions;

            IAssetDefaultConditionLayer assetDefaultLayer =
                await _assetDefaultConditionLayerRepository.GetAsync();

            IEnumerable<IAssetCondition> defaultLayerConditions
                = await _assetConditionRepository.GetAsync(assetDefaultLayer.Id);

            var map = new Dictionary<string, AssetCondition>();

            // Initialize asset conditions using default layer conditions
            foreach (IAssetCondition condition in defaultLayerConditions)
            {
                map[condition.Asset] = Mapper.Map<AssetCondition>(condition);
            }

            // Merge client conditions layers
            IEnumerable<IAssetConditionLayer> layers = await GetLayersAsync(clientId);

            foreach (IAssetConditionLayer layer in layers.OrderBy(e => e.Priority))
            {
                var explicitAssets = new HashSet<string>();

                IEnumerable<IAssetCondition> conditions = await _assetConditionRepository.GetAsync(layer.Id);
                
                // Apply explicit assets conditions
                foreach (IAssetCondition condition in conditions)
                {
                    if (!map.TryGetValue(condition.Asset, out var value))
                        map[condition.Asset] = Mapper.Map<AssetCondition>(condition);
                    else
                        value.Apply(condition);

                    explicitAssets.Add(condition.Asset);
                }

                IAssetConditionSettings defaultAssetCondition = await _assetDefaultConditionRepository.GetAsync(layer.Id);

                if(defaultAssetCondition == null)
                    continue;

                // Apply implicit assets conditions
                IEnumerable<string> implicitAssets = map.Keys.Where(o => !explicitAssets.Contains(o));

                foreach (string asset in implicitAssets)
                {
                    map[asset].Apply(defaultAssetCondition);
                }
            }

            assetConditions = map.Values.Cast<IAssetCondition>().ToList();

            // Update asset conditions cache
            await _cacheManager.SaveAssetConditionsForClientAsync(clientId, assetConditions);

            return assetConditions;
        }

        public async Task<IAssetConditionLayerSettings> GetAssetConditionsLayerSettingsByClient(string clientId)
        {
            IEnumerable<IAssetConditionLayer> layers = await GetClientLayers(clientId);
            IAssetConditionLayerSettings defaultLayer = await _assetDefaultConditionLayerRepository.GetAsync();

            var settings = Mapper.Map<AssetConditionLayerSettings>(defaultLayer);

            foreach (IAssetConditionLayer layer in layers.OrderBy(o => o.Priority))
            {
                settings.Apply(layer);
            }

            return settings;
        }

        private async Task<IEnumerable<IAssetConditionLayer>> GetLayersAsync(string clientId)
        {
            IEnumerable<string> layerIds = await _assetConditionLayerLinkClientRepository.GetLayersAsync(clientId);
            return await _assetConditionLayerRepository.GetAsync(layerIds);
        }
    }
}
