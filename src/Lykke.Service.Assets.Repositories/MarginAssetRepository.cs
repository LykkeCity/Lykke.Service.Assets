using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class MarginAssetRepository : IMarginAssetRepository
    {
        private readonly INoSQLTableStorage<MarginAssetEntity> _marginAssetTable;


        public MarginAssetRepository(
            INoSQLTableStorage<MarginAssetEntity> marginAssetTable)
        {
            _marginAssetTable = marginAssetTable;
        }


        public async Task AddAsync(IMarginAsset marginAsset)
        {
            var entity = Mapper.Map<MarginAssetEntity>(marginAsset);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(marginAsset.Id);

            await _marginAssetTable.InsertAsync(entity);
        }

        public async Task<IEnumerable<IMarginAsset>> GetAllAsync()
        {
            return await _marginAssetTable.GetDataAsync(GetPartitionKey());
        }

        public async Task<IMarginAsset> GetAsync(string id)
        {
            return await _marginAssetTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task RemoveAsync(string id)
        {
            await _marginAssetTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpdateAsync(IMarginAsset marginAsset)
        {
            await _marginAssetTable.MergeAsync(GetPartitionKey(), GetRowKey(marginAsset.Id), x =>
            {
                Mapper.Map(marginAsset, x);

                return x;
            });
        }

        private static string GetPartitionKey()
            => "MarginAsset";

        private static string GetRowKey(string id)
            => id;
    }
}