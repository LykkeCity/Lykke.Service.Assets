using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;


namespace Lykke.Service.Assets.Repositories
{
    public class AssetPairRepository : IAssetPairRepository
    {
        private readonly INoSQLTableStorage<AssetPairEntity> _assetPairTable;


        public AssetPairRepository(
            INoSQLTableStorage<AssetPairEntity> assetPairTable)
        {
            _assetPairTable = assetPairTable;
        }


        public async Task<IEnumerable<IAssetPair>> GetAllAsync()
        {
            return await _assetPairTable.GetDataAsync();
        }

        public async Task<IAssetPair> GetAsync(string id)
        {
            return await _assetPairTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task RemoveAsync(string id)
        {
            await _assetPairTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpsertAsync(IAssetPair assetPair)
        {
            var entity = Mapper.Map<AssetPairEntity>(assetPair);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(assetPair.Id);

            await _assetPairTable.InsertAsync(entity);
        }

        public static string GetPartitionKey()
            => "AssetPair";

        public static string GetRowKey(string id)
            => id;
    }
}