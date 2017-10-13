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
    public class AssetGroupRepository : IAssetGroupRepository
    {
        private readonly INoSQLTableStorage<AssetGroupEntity> _assetGroupTable;


        public AssetGroupRepository(
            INoSQLTableStorage<AssetGroupEntity> assetGroupTable)
        {
            _assetGroupTable = assetGroupTable;
        }


        public async Task AddAsync(IAssetGroup group)
        {
            var entity = Mapper.Map<AssetGroupEntity>(group);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(group.Name);

            await _assetGroupTable.InsertAsync(entity);
        }

        public async Task<IEnumerable<IAssetGroup>> GetAllAsync()
        {
            var entities = await _assetGroupTable.GetDataAsync(GetPartitionKey());

            return entities.Select(Mapper.Map<AssetGroupDto>);
        }

        public async Task<IAssetGroup> GetAsync(string groupName)
        {
            var entity = await _assetGroupTable.GetDataAsync(GetPartitionKey(), GetRowKey(groupName));

            return Mapper.Map<AssetGroupDto>(entity);
        }

        public async Task RemoveAsync(string groupName)
        {
            await _assetGroupTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(groupName));
        }

        public async Task UpdateAsync(IAssetGroup group)
        {
            await _assetGroupTable.MergeAsync(GetPartitionKey(), GetRowKey(group.Name), x =>
            {
                Mapper.Map(group, x);

                return x;
            });
        }

        private static string GetPartitionKey()
            => "AssetGroup";

        private static string GetRowKey(string groupName)
            => groupName;
    }
}
