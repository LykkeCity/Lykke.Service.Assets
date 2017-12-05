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

        public static string GetPartitionKey(string clientId)
        {
            return clientId;
        }

        public static string GetRowKey(string layerId)
        {
            return layerId;
        }

        public async Task<IReadOnlyList<string>> GetAllLayersByClientAsync(string clientId)
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
            var clients = await _table.GetDataRowKeysOnlyAsync(new[] {GetRowKey(layerId)});
            await _table.DeleteAsync(clients);
        }
    }
}
