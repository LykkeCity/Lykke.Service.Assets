using System.Collections.Generic;
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
        private readonly INoSQLTableStorage<AssetConditionEntity> _assetConditionTable;
        private readonly INoSQLTableStorage<AssetConditionLayerEntity> _assetConditionLayerTable;

        public AssetConditionLayerRepository(
            INoSQLTableStorage<AssetConditionEntity> assetConditionTable,
            INoSQLTableStorage<AssetConditionLayerEntity> assetConditionLayerTable)
        {
            _assetConditionTable = assetConditionTable;
            _assetConditionLayerTable = assetConditionLayerTable;
        }

        public async Task<IReadOnlyList<IAssetConditionLayer>> GetByIdsAsync(IEnumerable<string> layerIds)
        {
            List<string> layerIdsList = layerIds.ToList();

            IEnumerable<AssetConditionLayerEntity> layers =
                await _assetConditionLayerTable.GetDataAsync(GetAssetConditionLayerPartitionKey(),
                    layerIdsList.Select(GetAssetConditionLayerRowKey));

            IList<AssetConditionEntity> assetConditions =
                (await _assetConditionTable.GetDataAsync(layerIdsList.Select(GetAssetConditionPartitionKey)))
                .ToList();

            var result = new List<AssetConditionLayerDto>();

            foreach (AssetConditionLayerEntity layer in layers)
            {
                var dto = new AssetConditionLayerDto
                {
                    Id = layer.Id,
                    Description = layer.Description,
                    Priority = (decimal)layer.Priority,
                    SwiftDepositEnabled = layer.SwiftDepositEnabled,
                    ClientsCanCashInViaBankCards = layer.ClientsCanCashInViaBankCards,
                    AssetConditions = assetConditions.Where(e => e.Layer == layer.Id)
                        .ToDictionary(e => e.Asset, e => e as IAssetCondition)
                };

                result.Add(dto);
            }

            return result;
        }

        public async Task<IReadOnlyList<IAssetConditionLayer>> GetLayerListAsync()
        {
            IEnumerable<AssetConditionLayerEntity> layers =
                await _assetConditionLayerTable.GetDataAsync(GetAssetConditionLayerPartitionKey());

            var result = new List<AssetConditionLayerDto>();

            foreach (AssetConditionLayerEntity layer in layers)
            {
                var dto = new AssetConditionLayerDto
                {
                    Id = layer.Id,
                    Description = layer.Description,
                    Priority = (decimal) layer.Priority,
                    SwiftDepositEnabled = layer.SwiftDepositEnabled,
                    ClientsCanCashInViaBankCards = layer.ClientsCanCashInViaBankCards
                };

                result.Add(dto);
            }

            return result;
        }

        public async Task InsertOrUpdateAssetConditionsToLayer(string layerId, IAssetCondition assetCondition)
        {
            var entity = new AssetConditionEntity(
                GetAssetConditionPartitionKey(layerId),
                GetAssetConditionRowKey(assetCondition.Asset),
                layerId,
                assetCondition.Asset,
                assetCondition.AvailableToClient,
                assetCondition.Regulation);

            await _assetConditionTable.InsertOrReplaceAsync(entity);
        }

        public async Task InsetLayerAsync(IAssetConditionLayer layer)
        {
            var entity = new AssetConditionLayerEntity(
                GetAssetConditionLayerPartitionKey(),
                GetAssetConditionLayerRowKey(layer.Id.Trim().ToLower()),
                layer.Priority,
                layer.Description,
                layer.ClientsCanCashInViaBankCards,
                layer.SwiftDepositEnabled);

            await _assetConditionLayerTable.InsertAsync(entity);

            if (layer.AssetConditions != null)
            {
                foreach (var assetCondition in layer.AssetConditions.Values)
                {
                    await InsertOrUpdateAssetConditionsToLayer(layer.Id, assetCondition);
                }
            }
        }

        public async Task UpdateLayerAsync(IAssetConditionLayer layer)
        {
            await _assetConditionLayerTable.ReplaceAsync(GetAssetConditionLayerPartitionKey(),
                GetAssetConditionLayerRowKey(layer.Id),
                current => current.Apply(layer.Priority, layer.Description, layer.ClientsCanCashInViaBankCards,
                    layer.SwiftDepositEnabled));
        }

        public async Task DeleteLayerAsync(string layerId)
        {
            var assetConditions = await _assetConditionTable.GetDataAsync(GetAssetConditionPartitionKey(layerId));
            await _assetConditionTable.DeleteAsync(assetConditions);

            await _assetConditionLayerTable.DeleteAsync(GetAssetConditionLayerPartitionKey(),
                GetAssetConditionLayerRowKey(layerId));
        }
        
        private static string GetAssetConditionLayerPartitionKey()
        {
            return "ConditionLayer";
        }

        private static string GetAssetConditionLayerRowKey(string layerId)
        {
            return layerId;
        }

        private static string GetAssetConditionPartitionKey(string layerId)
        {
            return $"Condition_{layerId}";
        }

        private static string GetAssetConditionRowKey(string asset)
        {
            return asset;
        }
    }
}
