using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;


namespace Lykke.Service.Assets.Repositories
{
    public class AssetGroupClientLinkRepository : IAssetGroupClientLinkRepository
    {
        private const string PartitionKeyPrefix = "GroupClientLink_";

        private readonly INoSQLTableStorage<AssetGroupEntity> _assetGroupTable;


        public AssetGroupClientLinkRepository(
            INoSQLTableStorage<AssetGroupEntity> assetGroupTable)
        {
            _assetGroupTable = assetGroupTable;
        }


        public async Task AddAsync(IAssetGroupClientLink groupClientLink)
        {
            var entity = Mapper.Map<AssetGroupEntity>(groupClientLink);

            entity.PartitionKey = GetPartitionKey(groupClientLink.ClientId);
            entity.RowKey       = GetRowKey(groupClientLink.GroupName);

            await _assetGroupTable.InsertAsync(entity);
        }

        public async Task AddOrReplaceAsync(IAssetGroupClientLink groupClientLink)
        {
            var entity = Mapper.Map<AssetGroupEntity>(groupClientLink);

            entity.PartitionKey = GetPartitionKey(groupClientLink.ClientId);
            entity.RowKey = GetRowKey(groupClientLink.GroupName);

            await _assetGroupTable.InsertOrReplaceAsync(entity);
        }

        public async Task<IAssetGroupClientLink> GetAsync(string clientId, string groupName)
        {
            var entity = await _assetGroupTable.GetDataAsync(GetPartitionKey(clientId), GetRowKey(groupName));

            return Mapper.Map<AssetGroupClientLinkDto>(entity);
        }

        public async Task<IEnumerable<IAssetGroupClientLink>> GetAllAsync()
        {
            var entities = (await _assetGroupTable.GetDataAsync())
                .Where(x => x.PartitionKey.StartsWith(PartitionKeyPrefix));

            return entities.Select(Mapper.Map<AssetGroupClientLinkDto>);
        }

        public async Task<IEnumerable<IAssetGroupClientLink>> GetAllAsync(string clientId)
        {
            var entities = await _assetGroupTable.GetDataAsync(GetPartitionKey(clientId));

            return entities.Select(Mapper.Map<AssetGroupClientLinkDto>);
        }

        public async Task RemoveAsync(string clientId, string groupName)
        {
            await _assetGroupTable.DeleteIfExistAsync(GetPartitionKey(clientId), GetRowKey(groupName));
        }

        public async Task UpdateAsync(string clientId, IAssetGroup group)
        {
            await _assetGroupTable.MergeAsync(GetPartitionKey(clientId), GetRowKey(group.Name), x =>
            {
                Mapper.Map(group, x);

                return x;
            });
        }

        private static string GetPartitionKey(string clientId)
            => $"{PartitionKeyPrefix}{clientId}";

        private static string GetRowKey(string groupName)
            => groupName;
    }
}
