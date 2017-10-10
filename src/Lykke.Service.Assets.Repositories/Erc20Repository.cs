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
    public class Erc20AssetRepository : IErc20AssetRepository
    {
        private const string _assetPartition = "ErcAssetId";
        private readonly INoSQLTableStorage<AzureIndex> _indexAssetIdTable;
        private readonly INoSQLTableStorage<Erc20AssetEntity> _erc20AssetEntityTable;

        public Erc20AssetRepository(INoSQLTableStorage<Erc20AssetEntity> erc20AssetEntityTable, 
            INoSQLTableStorage<AzureIndex> indexAssetIdTable)
        {
            _indexAssetIdTable          = indexAssetIdTable;
            _erc20AssetEntityTable      = erc20AssetEntityTable;
        }

        public async Task<IEnumerable<IErc20Asset>> GetAllAsync()
        {
            var allEntities = await _erc20AssetEntityTable.GetDataAsync();

            return allEntities;
        }

        public async Task<IErc20Asset> GetByAddressAsync(string tokenAddress)
        {
            IErc20Asset entity = await _erc20AssetEntityTable.GetDataAsync(Erc20AssetEntity.GeneratePartitionKey(), 
                Erc20AssetEntity.GenerateRowKey(tokenAddress));

            return entity;
        }

        public async Task<IErc20Asset> GetByAssetIdAsync(string assetId)
        {
            AzureIndex indexes   = await _indexAssetIdTable.GetDataAsync(_assetPartition, assetId);
            IErc20Asset entity = await _erc20AssetEntityTable.GetDataAsync(indexes);

            return entity;
        }

        public async Task<IEnumerable<IErc20Asset>> GetByAssetIdAsync(string[] assetIds)
        {
            IEnumerable<AzureIndex> indexes   = await _indexAssetIdTable.GetDataAsync(_assetPartition, assetIds);
            IEnumerable<IErc20Asset> entities = await _erc20AssetEntityTable.GetDataAsync(indexes);

            return entities;
        }

        public async Task SaveAsync(IErc20Asset erc20Asset)
        {
            Erc20AssetEntity entity = Mapper.Map<Erc20AssetEntity>(erc20Asset);
            Fill(entity);

            await _erc20AssetEntityTable.InsertOrMergeAsync(entity);
            await _indexAssetIdTable.InsertOrReplaceAsync(new AzureIndex(_assetPartition, erc20Asset.AssetId, entity));
        }

        public async Task UpdateAsync(IErc20Asset erc20Asset)
        {
            Erc20AssetEntity entity = Mapper.Map<Erc20AssetEntity>(erc20Asset);
            Fill(entity);

            await _erc20AssetEntityTable.InsertOrReplaceAsync(entity);
            await _indexAssetIdTable.InsertOrReplaceAsync(new AzureIndex(_assetPartition, erc20Asset.AssetId, entity));
        }

        private void Fill(Erc20AssetEntity entity)
        {
            entity.PartitionKey = Erc20AssetEntity.GeneratePartitionKey();
            entity.RowKey = Erc20AssetEntity.GenerateRowKey(entity.Address);
        }
    }
}
