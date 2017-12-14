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

        public AssetConditionService(
            IAssetRepository assetRepository,
            IAssetConditionLayerRepository assetConditionLayerRepository,
            IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository,
            IAssetConditionDefaultLayerRepository assetConditionDefaultLayerRepository)
        {
            _assetRepository = assetRepository;
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetConditionLayerLinkClientRepository = assetConditionLayerLinkClientRepository;
            _assetConditionDefaultLayerRepository = assetConditionDefaultLayerRepository;
        }

        public async Task<IReadOnlyDictionary<string, IAssetCondition>> GetAssetConditionsByClient(string clientId)
        {
            IReadOnlyList<IAssetConditionLayer> layers = await GetLayersAsync(clientId);
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

            return result.ToDictionary(e => e.Key, e => e.Value as IAssetCondition);
        }

        public async Task<IAssetConditionLayerSettings> GetAssetConditionsLayerSettingsByClient(string clientId)
        {
            IReadOnlyList<IAssetConditionLayer> layers = await GetLayersAsync(clientId);
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

        private async Task<IReadOnlyList<IAssetConditionLayer>> GetLayersAsync(string clientId)
        {
            IReadOnlyList<string> layersIds =
                await _assetConditionLayerLinkClientRepository.GetAllLayersByClientAsync(clientId);

            return await _assetConditionLayerRepository.GetByIdsAsync(layersIds);
        }
    }
}
