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
    public class ClientAssetGroupLinkRepository : IClientAssetGroupLinkRepository
    {
        private readonly INoSQLTableStorage<AssetGroupEntity> _assetGroupTable;


        public ClientAssetGroupLinkRepository(
            INoSQLTableStorage<AssetGroupEntity> assetGroupTable)
        {
            _assetGroupTable = assetGroupTable;
        }


        public async Task AddAsync(IClientAssetGroupLink clientGroupLink)
        {
            var entity = Mapper.Map<AssetGroupEntity>(clientGroupLink);

            entity.PartitionKey = GetPartitionKey(clientGroupLink.GroupName);
            entity.RowKey       = GetRowKey(clientGroupLink.ClientId);

            await _assetGroupTable.InsertAsync(entity);
        }

        public async Task<IClientAssetGroupLink> GetAsync(string clientId, string groupName)
        {
            var entity = await _assetGroupTable.GetDataAsync(GetPartitionKey(groupName), GetRowKey(clientId));

            return Mapper.Map<ClientAssetGroupLink>(entity);
        }

        public async Task<IEnumerable<IClientAssetGroupLink>> GetAllAsync()
        {
            var entities = await _assetGroupTable.GetDataAsync();

            return entities.Select(Mapper.Map<ClientAssetGroupLink>);
        }

        public async Task<IEnumerable<IClientAssetGroupLink>> GetAllAsync(string groupName)
        {
            var entities = await _assetGroupTable.GetDataAsync(GetPartitionKey(groupName));

            return entities.Select(Mapper.Map<ClientAssetGroupLink>);
        }

        public async Task RemoveAsync(string clientId, string groupName)
        {
            await _assetGroupTable.DeleteIfExistAsync(GetPartitionKey(groupName), GetRowKey(clientId));
        }

        public async Task UpdateAsync(string clientId, IAssetGroup group)
        {
            await _assetGroupTable.MergeAsync(GetPartitionKey(clientId), GetRowKey(group.Name), x =>
            {
                Mapper.Map(group, x);

                return x;
            });
        }

        private static string GetPartitionKey(string groupName)
            => $"ClientGroupLink_{groupName}";

        private static string GetRowKey(string clientId)
            => clientId;
    }
}