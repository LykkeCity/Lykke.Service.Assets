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
    public class AssetGroupAssetLinkRepository : IAssetGroupAssetLinkRepository
    {
        private readonly INoSQLTableStorage<AssetGroupEntity> _assetGroupTable;


        public AssetGroupAssetLinkRepository(
            INoSQLTableStorage<AssetGroupEntity> assetGroupTable)
        {
            _assetGroupTable = assetGroupTable;
        }


        public async Task AddAsync(IAssetGroupAssetLink assetLink)
        {
            var entity = Mapper.Map<AssetGroupEntity>(assetLink);

            entity.PartitionKey = GetPartitionKey(assetLink.GroupName);
            entity.RowKey       = GetRowKey(assetLink.AssetId);

            await _assetGroupTable.InsertAsync(entity);
        }

        public async Task<IAssetGroupAssetLink> GetAsync(string assetId, string groupName)
        {
            var entity = await _assetGroupTable.GetDataAsync(GetPartitionKey(groupName), GetRowKey(assetId));

            return Mapper.Map<AssetGroupAssetLinkDto>(entity);
        }

        public async Task<IEnumerable<IAssetGroupAssetLink>> GetAllAsync()
        {
            var entities = await _assetGroupTable.GetDataAsync();

            return entities.Select(Mapper.Map<AssetGroupAssetLinkDto>);
        }

        public async Task<IEnumerable<IAssetGroupAssetLink>> GetAllAsync(string groupName)
        {
            var entities = await _assetGroupTable.GetDataAsync(GetPartitionKey(groupName));

            return entities.Select(Mapper.Map<AssetGroupAssetLinkDto>);
        }

        public async Task RemoveAsync(string assetId, string groupName)
        {
            await _assetGroupTable.DeleteIfExistAsync(GetPartitionKey(groupName), GetRowKey(assetId));
        }

        public async Task UpdateAsync(string assetId, IAssetGroup group)
        {
            await _assetGroupTable.MergeAsync(GetPartitionKey(group.Name), GetRowKey(assetId), x =>
            {
                Mapper.Map(group, x);

                return x;
            });
        }

        private static string GetPartitionKey(string groupName)
            => $"AssetLink_{groupName}";

        private static string GetRowKey(string assetId)
            => assetId;
    }
}