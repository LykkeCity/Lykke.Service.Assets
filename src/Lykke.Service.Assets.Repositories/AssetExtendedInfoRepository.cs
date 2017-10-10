using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;


namespace Lykke.Service.Assets.Repositories
{
    public class AssetExtendedInfoRepository : IAssetExtendedInfoRepository
    {
        private readonly INoSQLTableStorage<AssetExtendedInfoEntity> _assetExtendedInfoTable;

        public AssetExtendedInfoRepository(
            INoSQLTableStorage<AssetExtendedInfoEntity> assetExtendedInfoTable)
        {
            _assetExtendedInfoTable = assetExtendedInfoTable;
        }


        public async Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync()
        {
            return await _assetExtendedInfoTable.GetDataAsync();
        }

        public async Task<IAssetExtendedInfo> GetAsync(string id)
        {
            return await _assetExtendedInfoTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task RemoveAsync(string id)
        {
            await _assetExtendedInfoTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpsertAsync(IAssetExtendedInfo assetInfo)
        {
            var entity = Mapper.Map<AssetExtendedInfoEntity>(assetInfo);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(assetInfo.Id);

            await _assetExtendedInfoTable.InsertOrReplaceAsync(entity);
        }

        private static string GetPartitionKey()
            => "aei";

        private static string GetRowKey(string id)
            => id;
    }
}
