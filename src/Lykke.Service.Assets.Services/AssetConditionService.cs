using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetConditionService : IAssetConditionService
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetConditionLayerRepository _assetConditionLayerRepository;
        private readonly IAssetConditionLayerLinkClientRepository _assetConditionLayerLinkClientRepository;
        private readonly IAssetConditionDefaultLayerRepository _assetConditionDefaultLayerRepository;
        private readonly IAssetsForClientCacheManager _cacheManager;

        public AssetConditionService(
            IAssetRepository assetRepository,
            IAssetConditionLayerRepository assetConditionLayerRepository,
            IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository,
            IAssetConditionDefaultLayerRepository assetConditionDefaultLayerRepository,
            IAssetsForClientCacheManager cacheManager)
        {
            _assetRepository = assetRepository;
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetConditionLayerLinkClientRepository = assetConditionLayerLinkClientRepository;
            _assetConditionDefaultLayerRepository = assetConditionDefaultLayerRepository;
            _cacheManager = cacheManager;
        }

        public async Task<IReadOnlyList<IAssetConditionLayer>> GetLayersAsync()
        {
            return await _assetConditionLayerRepository.GetLayerListAsync();
        }

        public async Task<IAssetConditionLayer> GetLayerAsync(string layerId)
        {
            IReadOnlyList<IAssetConditionLayer> layers =
                await _assetConditionLayerRepository.GetByIdsAsync(new[] {layerId});

            return layers.FirstOrDefault();
        }

        public async Task AddAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _assetConditionLayerRepository.InsertOrUpdateAssetConditionsToLayerAsync(layerId, assetCondition);

            await _cacheManager.ClearCacheAsync("Added asset condition");
        }

        public async Task UpdateAssetConditionAsync(string layerId, IAssetCondition assetCondition)
        {
            await _assetConditionLayerRepository.InsertOrUpdateAssetConditionsToLayerAsync(layerId, assetCondition);

            await _cacheManager.ClearCacheAsync("Updated asset condition");
        }

        public async Task DeleteAssetConditionAsync(string layerId, string assetId)
        {
            await _assetConditionLayerRepository.DeleteAssetConditionsAsync(layerId, assetId);

            await _cacheManager.ClearCacheAsync("Deleted asset condition");
        }

        public async Task AddLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerRepository.InsetLayerAsync(layer);
        }

        public async Task UpdateLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerRepository.UpdateLayerAsync(layer);

            await _cacheManager.ClearCacheAsync("Updated condition layer");
        }

        public async Task DeleteLayerAsync(string layerId)
        {
            await _assetConditionLayerLinkClientRepository.RemoveLayerFromClientsAsync(layerId);

            await _assetConditionLayerRepository.DeleteLayerAsync(layerId);

            await _cacheManager.ClearCacheAsync("Deleted condition layer");
        }

        public async Task<IReadOnlyList<IAssetConditionLayer>> GetClientLayers(string clientId)
        {
            IReadOnlyList<string> layerIds = await _assetConditionLayerLinkClientRepository.GetAllLayersByClientAsync(clientId);

            return await _assetConditionLayerRepository.GetByIdsAsync(layerIds);
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

        public async Task<IReadOnlyDictionary<string, IAssetCondition>> GetAssetConditionsByClient(string clientId)
        {
            IList<IAssetCondition> assetConditions = await _cacheManager.TryGetAssetConditionsForClientAsync(clientId);

            if (assetConditions != null)
                return assetConditions.ToDictionary(o => o.Asset, o => o);

            IReadOnlyList<IAssetConditionLayer> layers = await GetClientLayers(clientId);
            IEnumerable<IAsset> assets = await _assetRepository.GetAllAsync(true);
            IAssetConditionDefaultLayer defaultLayer = await _assetConditionDefaultLayerRepository.GetAsync();

            // Create conditions for all assets using default settings
            Dictionary<string, AssetCondition> result = assets
                .Select(o => new AssetCondition(o.Id)
                {
                    Asset = o.Id,
                    Regulation = defaultLayer?.Regulation,
                    AvailableToClient = defaultLayer?.AvailableToClient
                })
                .ToDictionary(o => o.Asset, o => o);

            foreach (IAssetConditionLayer layer in layers.OrderBy(e => e.Priority))
            {
                foreach (KeyValuePair<string, IAssetCondition> condition in layer.AssetConditions)
                {
                    if (!result.TryGetValue(condition.Key, out var value))
                    {
                        value = new AssetCondition(condition.Key);
                        result[condition.Key] = value;
                    }
                    value.Apply(condition.Value);
                }
            }

            await _cacheManager.SaveAssetConditionsForClientAsync(clientId,
                result.Values.Cast<IAssetCondition>().ToList());

            return result.ToDictionary(e => e.Key, e => e.Value as IAssetCondition);
        }

        public async Task<IAssetConditionLayerSettings> GetAssetConditionsLayerSettingsByClient(string clientId)
        {
            IReadOnlyList<IAssetConditionLayer> layers = await GetClientLayers(clientId);
            IAssetConditionDefaultLayer defaultLayer = await _assetConditionDefaultLayerRepository.GetAsync();

            var assetConditionLayerSettings = new AssetConditionLayerSettings
            {
                ClientsCanCashInViaBankCards = defaultLayer?.ClientsCanCashInViaBankCards,
                SwiftDepositEnabled = defaultLayer?.SwiftDepositEnabled
            };

            IAssetConditionLayer layerSwiftDepositEnabled = layers
                .Where(e => e.SwiftDepositEnabled.HasValue)
                .OrderByDescending(e => e.Priority)
                .FirstOrDefault();

            if (layerSwiftDepositEnabled != null)
                assetConditionLayerSettings.SwiftDepositEnabled = layerSwiftDepositEnabled.SwiftDepositEnabled;

            IAssetConditionLayer layerClientsCanCashInViaBankCards = layers
                .Where(e => e.ClientsCanCashInViaBankCards.HasValue)
                .OrderByDescending(e => e.Priority)
                .FirstOrDefault();

            if (layerClientsCanCashInViaBankCards != null)
                assetConditionLayerSettings.ClientsCanCashInViaBankCards = layerClientsCanCashInViaBankCards.ClientsCanCashInViaBankCards;

            return assetConditionLayerSettings;
        }
    }
}
