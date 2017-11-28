﻿using System.Collections.Generic;
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
        private readonly IAssetConditionLayerRepository _assetConditionLayerRepository;
        private readonly IAssetConditionLayerLinkClientRepository _assetConditionLayerLinkClientRepository;

        public AssetConditionService(IAssetConditionLayerRepository assetConditionLayerRepository, 
            IAssetConditionLayerLinkClientRepository assetConditionLayerLinkClientRepository)
        {
            _assetConditionLayerRepository = assetConditionLayerRepository;
            _assetConditionLayerLinkClientRepository = assetConditionLayerLinkClientRepository;
        }

        public async Task<IReadOnlyDictionary<string, IAssetConditions>> GetAssetConditionsByClient(string clientId)
        {
            var layersIds = await _assetConditionLayerLinkClientRepository.GetAllLayersByClientAsync(clientId);
            var layers = await _assetConditionLayerRepository.GetByIdsAsync(layersIds);

            var result = new Dictionary<string, AssetConditions>();

            foreach (var layer in layers.OrderBy(e => e.Priority))
            {
                foreach (var condition in layer.AssetConditions)
                {
                    if (!result.TryGetValue(condition.Key, out var value))
                    {
                        value = new AssetConditions(condition.Key);
                        result[condition.Key] = value;
                    }
                    value.Apply(condition.Value);
                }
            }

            return result.ToDictionary(e => e.Key, e => e.Value as IAssetConditions);
        }
    }
}
