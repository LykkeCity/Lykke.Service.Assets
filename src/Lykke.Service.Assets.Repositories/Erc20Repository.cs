using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class Erc20TokenRepository : IErc20TokenRepository
    {
        private const string AssetIndexPartition = "Erc20TokenAssetId";

        private readonly INoSQLTableStorage<Erc20TokenEntity> _erc20AssetEntityTable;
        private readonly INoSQLTableStorage<AzureIndex>       _indexAssetIdTable;

        public Erc20TokenRepository(INoSQLTableStorage<Erc20TokenEntity> erc20AssetEntityTable,
            INoSQLTableStorage<AzureIndex> indexAssetIdTable)
        {
            _indexAssetIdTable     = indexAssetIdTable;
            _erc20AssetEntityTable = erc20AssetEntityTable;
        }

        
        public async Task AddAsync(IErc20Token erc20Token)
        {
            var entity = Mapper.Map<Erc20TokenEntity>(erc20Token);
            var index  = new AzureIndex(AssetIndexPartition, erc20Token.AssetId, entity);

            SetEntityKeys(entity);

            await _erc20AssetEntityTable.InsertOrReplaceAsync(entity);
            if (erc20Token.AssetId != null)
            {
                await _indexAssetIdTable.InsertOrReplaceAsync(index);
            }
        }

        public async Task<IEnumerable<IErc20Token>> GetAllAsync()
        {
            var allEntities = await _erc20AssetEntityTable.GetDataAsync();

            return allEntities;
        }

        public async Task<IErc20Token> GetByAddressAsync(string tokenAddress)
        {
            var entity = await _erc20AssetEntityTable.GetDataAsync(GetPartitionKey(), GetRowKey(tokenAddress));

            return entity;
        }

        public async Task<IErc20Token> GetByAssetIdAsync(string assetId)
        {
            var indexes = await _indexAssetIdTable.GetDataAsync(AssetIndexPartition, assetId);
            var entity  = await _erc20AssetEntityTable.GetDataAsync(indexes);

            return entity;
        }

        public async Task<IEnumerable<IErc20Token>> GetByAssetIdAsync(string[] assetIds)
        {
            var indexes  = await _indexAssetIdTable.GetDataAsync(AssetIndexPartition, assetIds);
            var entities = await _erc20AssetEntityTable.GetDataAsync(indexes);

            return entities;
        }
        
        public async Task UpdateAsync(IErc20Token erc20Token)
        {
            var entity = Mapper.Map<Erc20TokenEntity>(erc20Token);
            var index  = new AzureIndex(AssetIndexPartition, erc20Token.AssetId, entity);

            SetEntityKeys(entity);

            await _erc20AssetEntityTable.InsertOrMergeAsync(entity);

            if (erc20Token.AssetId != null)
            {
                await _indexAssetIdTable.InsertOrMergeAsync(index);
            }
        }

        private static void SetEntityKeys(Erc20TokenEntity entity)
        {
            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(entity.Address);
        }

        private static string GetPartitionKey()
            => "Erc20Token";

        private static string GetRowKey(string tokenAddress)
            => tokenAddress?.ToLower();
    }
}