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
    public class AssetConditionService : IAssetConditionService
    {
        private readonly IAssetConditionRepository _assetConditionRepository;
        private readonly IAssetConditionLayerRepository _assetConditionLayerRepository;
        private readonly IAssetConditionSettingsRepository _assetConditionSettingsRepository;
        private readonly IAssetConditionLayerSettingsRepository _assetConditionLayerSettingsRepository;
        private readonly IAssetConditionLayerLinkClientRepository _assetConditionLayerLinkClientRepository;
        private readonly IAssetsForClientCacheManager _cacheManager;

        public AssetConditionService(
            IAssetConditionRepository assetConditionRepository, 
            IAssetConditionLayerRepository assetConditionLayerRepository,
            IAssetConditionSettingsRepository assetConditionSettingsRepository,
            IAssetConditionLayerSettingsRepository assetConditionLayerSettingsRepository,
            IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository,
            IAssetsForClientCacheManager cacheManager)
        {
            _assetConditionRepository = assetConditionRepository;
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetConditionSettingsRepository = assetConditionSettingsRepository;
            _assetConditionLayerSettingsRepository = assetConditionLayerSettingsRepository;
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

            var model = Mapper.Map<AssetConditionLayer>(layer);

            model.AssetConditions = conditions.ToList();

            return layer;
        }

        public async Task AddAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _assetConditionRepository.InsertOrUpdateAsync(layerId, assetCondition);

            await _cacheManager.ClearCacheAsync("Added asset condition");
        }

        public async Task UpdateAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _assetConditionRepository.InsertOrUpdateAsync(layerId, assetCondition);

            await _cacheManager.ClearCacheAsync("Updated asset condition");
        }

        public async Task DeleteAssetConditionAsync(string layerId, string assetId)
        {
            await _assetConditionRepository.DeleteAsync(layerId, assetId);

            await _cacheManager.ClearCacheAsync("Deleted asset condition");
        }

        public async Task AddLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerRepository.InsertOrUpdateAsync(layer);
        }

        public async Task UpdateLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerRepository.InsertOrUpdateAsync(layer);

            await _cacheManager.ClearCacheAsync("Updated condition layer");
        }

        public async Task DeleteLayerAsync(string layerId)
        {
            await Task.WhenAll(
                _assetConditionLayerLinkClientRepository.RemoveLayerFromClientsAsync(layerId),
                _assetConditionRepository.DeleteAsync(layerId),
                _assetConditionLayerRepository.DeleteAsync(layerId));

            await _cacheManager.ClearCacheAsync("Deleted condition layer");
        }

        public async Task<IEnumerable<IAssetConditionLayer>> GetClientLayers(string clientId)
        {
            IReadOnlyList<string> layerIds = await _assetConditionLayerLinkClientRepository.GetAllLayersByClientAsync(clientId);

            IEnumerable<IAssetConditionLayer> layers = await _assetConditionLayerRepository.GetAsync(layerIds);

            var model = Mapper.Map<List<AssetConditionLayer>>(layers);

            foreach (AssetConditionLayer layer in model)
            {
                IEnumerable<IAssetCondition> conditions = await _assetConditionRepository.GetAsync(layer.Id);
                layer.AssetConditions = conditions.ToList();
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

            IEnumerable<IAssetConditionLayer> layers = await GetClientLayers(clientId);
            IAssetConditionSettings defaultAssetCondition = await _assetConditionSettingsRepository.GetAsync();

            var map = new Dictionary<string, AssetCondition>();

            foreach (IAssetConditionLayer layer in layers.OrderBy(e => e.Priority))
            {
                foreach (IAssetCondition condition in layer.AssetConditions)
                {
                    if (!map.TryGetValue(condition.Asset, out var value))
                    {
                        value = Mapper.Map<AssetCondition>(defaultAssetCondition);
                        value.Asset = condition.Asset;
                        map[condition.Asset] = value;
                    }

                    value.Apply(condition);
                }
            }

            assetConditions = map.Values.Cast<IAssetCondition>().ToList();

            await _cacheManager.SaveAssetConditionsForClientAsync(clientId, assetConditions);

            return assetConditions;
        }

        public async Task<IAssetConditionLayerSettings> GetAssetConditionsLayerSettingsByClient(string clientId)
        {
            IEnumerable<IAssetConditionLayer> layers = await GetClientLayers(clientId);
            IAssetConditionLayerSettings defaultLayer = await _assetConditionLayerSettingsRepository.GetAsync();

            var settings = Mapper.Map<AssetConditionLayerSettings>(defaultLayer);

            foreach (IAssetConditionLayer layer in layers.OrderBy(o => o.Priority))
            {
                settings.Apply(layer);
            }

            return settings;
        }
    }
}
