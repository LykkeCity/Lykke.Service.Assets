using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using AzureStorage.Tables.Templates.Index;
using AutoMapper;

namespace Lykke.Service.Assets.Repositories
{
    public class Erc20TokenRepository : IErc20TokenRepository
    {
        private const string _assetPartition = "ErcAssetId";
        private readonly INoSQLTableStorage<AzureIndex> _indexAssetIdTable;
        private readonly INoSQLTableStorage<Erc20TokenEntity> _erc20AssetEntityTable;

        public Erc20TokenRepository(INoSQLTableStorage<Erc20TokenEntity> erc20AssetEntityTable, 
            INoSQLTableStorage<AzureIndex> indexAssetIdTable)
        {
            _indexAssetIdTable          = indexAssetIdTable;
            _erc20AssetEntityTable      = erc20AssetEntityTable;
        }

        public async Task<IEnumerable<IErc20Token>> GetAllAsync()
        {
            var allEntities = await _erc20AssetEntityTable.GetDataAsync();

            return allEntities;
        }

        public async Task<IErc20Token> GetByAddressAsync(string tokenAddress)
        {
            IErc20Token entity = await _erc20AssetEntityTable.GetDataAsync(Erc20TokenEntity.GeneratePartitionKey(), 
                Erc20TokenEntity.GenerateRowKey(tokenAddress));

            return entity;
        }

        public async Task<IErc20Token> GetByAssetIdAsync(string assetId)
        {
            AzureIndex indexes   = await _indexAssetIdTable.GetDataAsync(_assetPartition, assetId);
            IErc20Token entity = await _erc20AssetEntityTable.GetDataAsync(indexes);

            return entity;
        }

        public async Task<IEnumerable<IErc20Token>> GetByAssetIdAsync(string[] assetIds)
        {
            IEnumerable<AzureIndex> indexes   = await _indexAssetIdTable.GetDataAsync(_assetPartition, assetIds);
            IEnumerable<IErc20Token> entities = await _erc20AssetEntityTable.GetDataAsync(indexes);

            return entities;
        }

        public async Task SaveAsync(IErc20Token erc20Token)
        {
            Erc20TokenEntity entity = Mapper.Map<Erc20TokenEntity>(erc20Token);
            Fill(entity);

            await _erc20AssetEntityTable.InsertOrMergeAsync(entity);
            await _indexAssetIdTable.InsertOrReplaceAsync(new AzureIndex(_assetPartition, erc20Token.AssetId, entity));
        }

        public async Task UpdateAsync(IErc20Token erc20Token)
        {
            Erc20TokenEntity entity = Mapper.Map<Erc20TokenEntity>(erc20Token);
            Fill(entity);

            await _erc20AssetEntityTable.InsertOrReplaceAsync(entity);
            await _indexAssetIdTable.InsertOrReplaceAsync(new AzureIndex(_assetPartition, erc20Token.AssetId, entity));
        }

        private void Fill(Erc20TokenEntity entity)
        {
            entity.PartitionKey = Erc20TokenEntity.GeneratePartitionKey();
            entity.RowKey = Erc20TokenEntity.GenerateRowKey(entity.Address);
        }
    }
}
