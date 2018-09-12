using AutoMapper;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Repositories
{
    public class Erc20TokenRepository : IErc20TokenRepository
    {
        private const string AssetIndexPartition = "Erc20TokenAssetId";
        private readonly INoSQLTableStorage<Erc20TokenEntity> _erc20AssetEntityTable;
        private readonly INoSQLTableStorage<AzureIndex> _indexAssetIdTable;

        public Erc20TokenRepository(INoSQLTableStorage<Erc20TokenEntity> erc20AssetEntityTable,
            INoSQLTableStorage<AzureIndex> indexAssetIdTable)
        {
            _indexAssetIdTable = indexAssetIdTable;
            _erc20AssetEntityTable = erc20AssetEntityTable;
        }


        public async Task AddAsync(IErc20Token erc20Token)
        {
            var entity = Mapper.Map<Erc20TokenEntity>(erc20Token);
            SetEntityKeys(entity);
            var index = new AzureIndex(AssetIndexPartition, erc20Token.AssetId, entity);

            await _erc20AssetEntityTable.InsertOrReplaceAsync(entity);
            if (erc20Token.AssetId != null)
            {
                await _indexAssetIdTable.InsertOrReplaceAsync(index);
            }
        }

        public async Task<IEnumerable<IErc20Token>> GetAllAsync()
        {
            return await _erc20AssetEntityTable.GetDataAsync(GetPartitionKey());
        }

        public async Task<IErc20Token> GetByTokenAddressAsync(string tokenAddress)
        {
            return await _erc20AssetEntityTable.GetDataAsync(GetPartitionKey(), GetRowKey(tokenAddress));
        }

        public async Task<IErc20Token> GetByAssetIdAsync(string assetId)
        {
            var index = await _indexAssetIdTable.GetDataAsync(AssetIndexPartition, assetId);
            var entity = await _erc20AssetEntityTable.GetDataAsync(index);

            return entity;
        }

        public async Task UpdateAsync(IErc20Token erc20Token)
        {
            var entity = Mapper.Map<Erc20TokenEntity>(erc20Token);
            SetEntityKeys(entity);
            var index = new AzureIndex(AssetIndexPartition, erc20Token.AssetId, entity);

            await _erc20AssetEntityTable.InsertOrMergeAsync(entity);

            if (erc20Token.AssetId != null)
            {
                await _indexAssetIdTable.InsertOrMergeAsync(index);
            }
        }

        public async Task<IEnumerable<IErc20Token>> GetAllWithAssetsAsync()
        {
            var indexes = await _indexAssetIdTable.GetDataAsync(AssetIndexPartition);

            return (await GetByAssetIndexesAsync(indexes))
                // Ensure, that our indexes are not corrupted. It's important, we have already faced problems with them.
                .Where(x => x?.AssetId != null);
        }

        private async Task<IEnumerable<IErc20Token>> GetByAssetIndexesAsync(IEnumerable<AzureIndex> assetIndexes)
        {
            var rowKeys = assetIndexes.Select(x => x.PrimaryRowKey)
                // Ensure, that our indexes are not corrupted. It's important, we have already faced problems with them.
                .Distinct();

            return await _erc20AssetEntityTable.GetDataAsync(GetPartitionKey(), rowKeys);
        }

        private static void SetEntityKeys(Erc20TokenEntity entity)
        {
            entity.PartitionKey = GetPartitionKey();
            entity.RowKey = GetRowKey(entity.Address);
        }

        private static string GetPartitionKey()
            => "Erc20Token";

        private static string GetRowKey(string tokenAddress)
            => tokenAddress?.ToLower();
    }
}
