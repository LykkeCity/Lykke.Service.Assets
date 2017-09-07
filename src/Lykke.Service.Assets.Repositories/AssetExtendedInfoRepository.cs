using System.Collections.Generic;
using System.Threading.Tasks;
using AzureStorage;
using Microsoft.WindowsAzure.Storage.Table;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetExtendedInfoEntity : TableEntity, IAssetExtendedInfo
    {
        public static string GeneratePartitionKey()
        {
            return "aei";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string AssetClass { get; set; }
        public string Description { get; set; }
        public string IssuerName { get; set; }
        public string NumberOfCoins { get; set; }
        public string MarketCapitalization { get; set; }
        public int PopIndex { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string FullName { get; set; }
        

        public static AssetExtendedInfoEntity Create(IAssetExtendedInfo src)
        {
            return new AssetExtendedInfoEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(src.Id),
                PopIndex = src.PopIndex,
                MarketCapitalization = src.MarketCapitalization,
                AssetClass = src.AssetClass,
                NumberOfCoins = src.NumberOfCoins,
                Description = src.Description,
                AssetDescriptionUrl = src.AssetDescriptionUrl,
                FullName = src.FullName
            };
        }
    }

    public class AssetExtendedInfoRepository : IAssetExtendedInfoRepository, IDictionaryRepository<IAssetExtendedInfo>
    {
        private readonly INoSQLTableStorage<AssetExtendedInfoEntity> _tableStorage;

        public AssetExtendedInfoRepository(INoSQLTableStorage<AssetExtendedInfoEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task SaveAsync(IAssetExtendedInfo src)
        {
            var newEntity = AssetExtendedInfoEntity.Create(src);
            return _tableStorage.InsertOrReplaceAsync(newEntity);
        }

        public async Task<IAssetExtendedInfo> GetAssetExtendedInfoAsync(string id)
        {
            var partitionKey = AssetExtendedInfoEntity.GeneratePartitionKey();
            var rowKey = AssetExtendedInfoEntity.GenerateRowKey(id);

            return await _tableStorage.GetDataAsync(partitionKey, rowKey);
        }

        public async Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync()
        {
            var partitionKey = AssetExtendedInfoEntity.GeneratePartitionKey();
            return await _tableStorage.GetDataAsync(partitionKey);

        }
    }
}
