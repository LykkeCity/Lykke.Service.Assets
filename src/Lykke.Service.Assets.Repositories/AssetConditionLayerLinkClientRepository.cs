using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AzureStorage;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionLayerLinkClientRepository : IAssetConditionLayerLinkClientRepository
    {
        private readonly INoSQLTableStorage<AssetConditionLayerLinkClientEntity> _table;

        public AssetConditionLayerLinkClientRepository(INoSQLTableStorage<AssetConditionLayerLinkClientEntity> table)
        {
            _table = table;
        }

        public async Task<IEnumerable<string>> GetLayersAsync(string clientId)
        {
            var links = await _table.GetDataAsync(GetPartitionKey(clientId));
            return links.Select(e => e.LayerId).ToList();
        }

        public async Task AddAsync(string clientId, string layerId)
        {
            await _table.InsertOrReplaceAsync(new AssetConditionLayerLinkClientEntity(GetPartitionKey(clientId), GetRowKey(layerId)));
        }

        public async Task RemoveAsync(string clientId, string layerId)
        {
            await _table.DeleteAsync(GetPartitionKey(clientId), GetRowKey(layerId));
        }

        public async Task RemoveLayerFromClientsAsync(string layerId)
        {
            var entities = await _table.GetDataRowKeysOnlyAsync(new[] {GetRowKey(layerId)});

            var tasks = entities.Select(x => _table.DeleteAsync(x)).ToArray();
            await Task.WhenAll(tasks);
        }

        private static string GetPartitionKey(string clientId)
            => clientId;

        private static string GetRowKey(string layerId)
            => layerId;
    }
}
