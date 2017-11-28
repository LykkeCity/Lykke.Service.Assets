using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Dynamic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionLayerRepository : IAssetConditionLayerRepository
    {
        private readonly INoSQLTableStorage<AssetConditionEntity> _assetAssetConditionTable;
        private readonly INoSQLTableStorage<AssetConditionLayerEntity> _assetConditionLayerTable;

        public AssetConditionLayerRepository(INoSQLTableStorage<AssetConditionEntity> assetAssetConditionTable, INoSQLTableStorage<AssetConditionLayerEntity> assetConditionLayerTable)
        {
            _assetAssetConditionTable = assetAssetConditionTable;
            _assetConditionLayerTable = assetConditionLayerTable;
        }

        public static string GetAssetConditionLayerPartitionKey()
        {
            return "ConditionLayer";
        }

        public static string GetAssetConditionLayerRowKey(string layerId)
        {
            return layerId;
        }

        public static string GetAssetConditionPartitionKey(string layerId)
        {
            return $"Condition_{layerId}";
        }

        public static string GetAssetConditionRowKey(string asset)
        {
            return asset;
        }

        public async Task<IReadOnlyList<IAssetConditionLayer>> GetByIdsAsync(IEnumerable<string> layerIds)
        {
            var layerIdsList = layerIds.ToList();
            var layers = await _assetConditionLayerTable.GetDataAsync(GetAssetConditionLayerPartitionKey(), layerIdsList.Select(GetAssetConditionLayerRowKey))
                ?? new List<AssetConditionLayerEntity>();

            var assetConditions = (await _assetAssetConditionTable.GetDataAsync(layerIdsList.Select(GetAssetConditionPartitionKey))).ToList();
            var result = new List<AssetConditionLayerDto>();

            foreach (var layer in layers)
            {
                var dto = new AssetConditionLayerDto();
                dto.Id = layer.Id;
                dto.Description = layer.Description;
                dto.Priority = layer.Priority;
                dto.AssetConditions = assetConditions.Where(e => e.Layer == layer.Id).ToDictionary(e => e.Asset, e => e as IAssetConditions);
                result.Add(dto);
            }

            return result;
        }
    }
}
