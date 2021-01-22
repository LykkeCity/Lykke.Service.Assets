using System;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Lykke.Service.Assets.NoSql.Models;

namespace Lykke.Service.Assets.Services
{
    public class AssetConditionService : IAssetConditionService, IStartable
    {
        private readonly IAssetConditionLayerRepository _assetConditionLayerRepository;
        private readonly IAssetDefaultConditionRepository _assetDefaultConditionRepository;
        private readonly IAssetDefaultConditionLayerRepository _assetDefaultConditionLayerRepository;
        private readonly IAssetConditionLayerLinkClientRepository _assetConditionLayerLinkClientRepository;
        private readonly IAssetsForClientCacheManager _cacheManager;
        private readonly ICachedAssetConditionsService _cachedAssetConditionsService;
        private readonly IMyNoSqlWriterWrapper<AssetConditionNoSql> _myNoSqlWriter;
        private readonly int _maxClientsInNoSqlCache;

        public AssetConditionService(
            IAssetConditionLayerRepository assetConditionLayerRepository,
            IAssetDefaultConditionRepository assetDefaultConditionRepository,
            IAssetDefaultConditionLayerRepository assetDefaultConditionLayerRepository,
            IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository,
            IAssetsForClientCacheManager cacheManager,
            ICachedAssetConditionsService cachedAssetConditionsService,
            IMyNoSqlWriterWrapper<AssetConditionNoSql> myNoSqlWriter,
            int maxClientsInNoSqlCache)
        {
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetDefaultConditionRepository = assetDefaultConditionRepository;
            _assetDefaultConditionLayerRepository = assetDefaultConditionLayerRepository;
            _assetConditionLayerLinkClientRepository = assetConditionLayerLinkClientRepository;
            _cacheManager = cacheManager;
            _cachedAssetConditionsService = cachedAssetConditionsService;
            _myNoSqlWriter = myNoSqlWriter;
            _maxClientsInNoSqlCache = maxClientsInNoSqlCache;
        }

        public async Task<IEnumerable<IAssetConditionLayer>> GetLayersAsync()
        {
            return await _assetConditionLayerRepository.GetAsync();
        }

        public async Task<IAssetConditionLayer> GetLayerAsync(string layerId)
        {
            var layer = await _assetConditionLayerRepository.GetAsync(layerId);

            if (layer == null)
                return null;

            var conditions = await _cachedAssetConditionsService.GetConditionsAsync(layer.Id);
            var defaultCondition = await _assetDefaultConditionRepository.GetAsync(layer.Id);

            var model = Mapper.Map<AssetConditionLayer>(layer);

            model.AssetConditions = conditions.ToList();
            model.AssetDefaultCondition = defaultCondition;

            return model;
        }

        public async Task<IAssetDefaultConditionLayer> GetDefaultLayerAsync()
        {
            var defaultLayer = await _assetDefaultConditionLayerRepository.GetAsync();

            var conditions = await _cachedAssetConditionsService.GetConditionsAsync(defaultLayer.Id);

            var model = Mapper.Map<AssetDefaultConditionLayer>(defaultLayer);

            model.AssetConditions = conditions.ToList();

            return model;
        }

        public async Task AddAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _myNoSqlWriter.Clear();
            await _cachedAssetConditionsService.AddAssetConditionAsync(layerId, assetCondition);
        }

        public async Task UpdateAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _myNoSqlWriter.Clear();
            await _cachedAssetConditionsService.AddAssetConditionAsync(layerId, assetCondition);
        }

        public async Task DeleteAssetConditionAsync(string layerId, string assetId)
        {
            await _myNoSqlWriter.Clear();
            await _cachedAssetConditionsService.DeleteAssetConditionAsync(layerId, assetId);
        }

        public async Task AddDefaultAssetConditionAsync(string layerId, IAssetDefaultCondition assetDefaultCondition)
        {
            await _myNoSqlWriter.Clear();

            await _assetDefaultConditionRepository.InsertOrReplaceAsync(layerId, assetDefaultCondition);

            await _cacheManager.ClearCacheAsync("Added default asset condition");
        }

        public async Task UpdateDefaultAssetConditionAsync(string layerId, IAssetDefaultCondition assetDefaultCondition)
        {
            await _myNoSqlWriter.Clear();

            await _assetDefaultConditionRepository.InsertOrReplaceAsync(layerId, assetDefaultCondition);

            await _cacheManager.ClearCacheAsync("Updated default asset condition");
        }

        public async Task DeleteDefaultAssetConditionAsync(string layerId)
        {
            await _myNoSqlWriter.Clear();

            await _assetDefaultConditionRepository.DeleteAsync(layerId);

            await _cacheManager.ClearCacheAsync("Deleted default asset condition");
        }

        public async Task AddLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerRepository.InsertOrReplaceAsync(layer);
        }

        public async Task UpdateLayerAsync(IAssetConditionLayer layer)
        {
            await _myNoSqlWriter.Clear();

            await _assetConditionLayerRepository.InsertOrReplaceAsync(layer);

            await _cacheManager.ClearCacheAsync("Updated condition layer");
        }

        public async Task DeleteLayerAsync(string layerId)
        {
            await _myNoSqlWriter.Clear();

            await Task.WhenAll(
                _assetConditionLayerLinkClientRepository.RemoveLayerFromClientsAsync(layerId),
                _cachedAssetConditionsService.DeleteAssetConditionsAsync(layerId),
                _assetConditionLayerRepository.DeleteAsync(layerId),
                _assetDefaultConditionRepository.DeleteAsync(layerId));

            await _cacheManager.ClearCacheAsync("Deleted condition layer");
        }

        public async Task UpdateDefaultLayerAsync(IAssetConditionLayerSettings settings)
        {
            await _myNoSqlWriter.Clear();

            await _assetDefaultConditionLayerRepository.InsertOrReplaceAsync(settings);

            await _cacheManager.ClearCacheAsync("Default asset condition layer changed");
        }

        public async Task<IEnumerable<IAssetConditionLayer>> GetClientLayers(string clientId)
        {
            var layers = await GetLayersAsync(clientId);

            var model = Mapper.Map<List<AssetConditionLayer>>(layers);

            foreach (var layer in model)
            {
                var conditions = await _cachedAssetConditionsService.GetConditionsAsync(layer.Id);
                var defaultCondition = await _assetDefaultConditionRepository.GetAsync(layer.Id);

                layer.AssetConditions = conditions.ToList();
                layer.AssetDefaultCondition = defaultCondition;
            }

            return model;
        }

        public async Task AddClientLayerAsync(string clientId, string layerId)
        {
            await _myNoSqlWriter.TryDeleteAsync(AssetConditionNoSql.GeneratePartitionKey(clientId), AssetConditionNoSql.GenerateRowKey());

            await _assetConditionLayerLinkClientRepository.AddAsync(clientId, layerId);

            await _cacheManager.RemoveClientFromCacheAsync(clientId);
        }

        public async Task RemoveClientLayerAsync(string clientId, string layerId)
        {
            await _myNoSqlWriter.TryDeleteAsync(AssetConditionNoSql.GeneratePartitionKey(clientId), AssetConditionNoSql.GenerateRowKey());

            await _assetConditionLayerLinkClientRepository.RemoveAsync(clientId, layerId);

            await _cacheManager.RemoveClientFromCacheAsync(clientId);
        }

        public async Task<IEnumerable<IAssetCondition>> GetAssetConditionsByClient(string clientId)
        {
            var assetConditions = await _cacheManager.TryGetAssetConditionsForClientAsync(clientId);

            if (assetConditions != null)
                return assetConditions;

            var assetDefaultLayer = await _cachedAssetConditionsService.GetDefaultLayerAsync();

            var defaultLayerConditions = await _cachedAssetConditionsService.GetConditionsAsync(assetDefaultLayer.Id);

            var map = new Dictionary<string, AssetCondition>();

            // Initialize asset conditions using default layer conditions
            foreach (var condition in defaultLayerConditions)
            {
                map[condition.Asset] = Mapper.Map<AssetCondition>(condition);
            }

            // Merge client conditions layers
            var layers = await GetLayersAsync(clientId);

            foreach (var layer in layers.OrderBy(e => e.Priority))
            {
                var explicitAssets = new HashSet<string>();

                var conditions = await _cachedAssetConditionsService.GetConditionsAsync(layer.Id);

                // Apply explicit assets conditions
                foreach (var condition in conditions)
                {
                    if (!map.TryGetValue(condition.Asset, out var value))
                        map[condition.Asset] = Mapper.Map<AssetCondition>(condition);
                    else
                        value.Apply(condition);

                    explicitAssets.Add(condition.Asset);
                }

                var defaultAssetCondition = await _cachedAssetConditionsService.GetDefaultConditionsAsync(layer.Id);

                if (defaultAssetCondition == null)
                    continue;

                // Apply implicit assets conditions
                var implicitAssets = map.Keys.Where(o => !explicitAssets.Contains(o));

                foreach (var asset in implicitAssets)
                {
                    map[asset].Apply(defaultAssetCondition);
                }
            }

            assetConditions = map.Values.Cast<IAssetCondition>().ToList();

            // Update asset conditions cache
            await _cacheManager.SaveAssetConditionsForClientAsync(clientId, assetConditions);

            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetConditionNoSql.Create(clientId, assetConditions));

            return assetConditions;
        }

        public async Task<IAssetConditionLayerSettings> GetAssetConditionsLayerSettingsByClient(string clientId)
        {
            var layers = await GetClientLayers(clientId);
            var defaultLayer = await _assetDefaultConditionLayerRepository.GetAsync();

            var settings = Mapper.Map<AssetConditionLayerSettings>(defaultLayer);

            foreach (var layer in layers.OrderBy(o => o.Priority))
            {
                settings.Apply(layer);
            }

            return settings;
        }

        private async Task<IEnumerable<IAssetConditionLayer>> GetLayersAsync(string clientId)
        {
            var layerIds = await _assetConditionLayerLinkClientRepository.GetLayersAsync(clientId);
            return await _assetConditionLayerRepository.GetAsync(layerIds);
        }

        public void Start()
        {
            _myNoSqlWriter.StartWithClearing(_maxClientsInNoSqlCache);
        }
    }
}
