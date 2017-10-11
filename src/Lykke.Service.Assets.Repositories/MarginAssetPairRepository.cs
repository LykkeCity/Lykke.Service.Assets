using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class MarginAssetPairRepository : IMarginAssetPairRepository
    {
        private readonly INoSQLTableStorage<MarginAssetPairEntity> _marginAssetPairTable;


        public MarginAssetPairRepository(
            INoSQLTableStorage<MarginAssetPairEntity> marginAssetPairTable)
        {
            _marginAssetPairTable = marginAssetPairTable;
        }


        public async Task AddAsync(IMarginAssetPair marginAssetPair)
        {
            var entity = Mapper.Map<MarginAssetPairEntity>(marginAssetPair);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(marginAssetPair.Id);

            await _marginAssetPairTable.InsertAsync(entity);
        }

        public async Task<IEnumerable<IMarginAssetPair>> GetAllAsync()
        {
            return await _marginAssetPairTable.GetDataAsync();
        }

        public async Task<IMarginAssetPair> GetAsync(string id)
        {
            return await _marginAssetPairTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task RemoveAsync(string id)
        {
            await _marginAssetPairTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpdateAsync(IMarginAssetPair marginAssetPair)
        {
            await _marginAssetPairTable.MergeAsync(GetPartitionKey(), GetRowKey(marginAssetPair.Id), x =>
            {
                Mapper.Map(marginAssetPair, x);

                return x;
            });
        }

        private static string GetPartitionKey()
        {
            return "MarginAssetPair";
        }

        private static string GetRowKey(string id)
        {
            return id;
        }
    }
}