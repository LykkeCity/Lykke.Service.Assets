using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetCategoryRepository : IAssetCategoryRepository
    {
        private readonly INoSQLTableStorage<AssetCategoryEntity> _assetCategoryTable;


        public AssetCategoryRepository(
            INoSQLTableStorage<AssetCategoryEntity> assetCategoryTable)
        {
            _assetCategoryTable = assetCategoryTable;
        }
        

        public async Task AddAsync(IAssetCategory assetCategory)
        {
            await _assetCategoryTable.InsertAsync(new AssetCategoryEntity
            {
                AndroidIconUrl = assetCategory.AndroidIconUrl,
                IosIconUrl     = assetCategory.IosIconUrl,
                Name           = assetCategory.Name,
                PartitionKey   = GetPartitionKey(),
                SortOrder      = assetCategory.SortOrder
            });
        }

        public async Task<IAssetCategory> GetAsync(string id)
        {
            return await _assetCategoryTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task<IEnumerable<IAssetCategory>> GetAllAsync()
        {
            return (await _assetCategoryTable.GetDataAsync())
                .OrderBy(x => x.SortOrder)
                .ThenBy(x => x.Name);
        }

        public async Task RemoveAsync(string id)
        {
            await _assetCategoryTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpdateAsync(IAssetCategory assetCategory)
        {
            await _assetCategoryTable.ReplaceAsync(GetPartitionKey(), GetRowKey(assetCategory.Id), x =>
            {
                x.AndroidIconUrl = assetCategory.AndroidIconUrl;
                x.IosIconUrl     = assetCategory.IosIconUrl;
                x.Name           = assetCategory.Name;
                x.SortOrder      = assetCategory.SortOrder;

                return x;
            });
        }

        private static string GetPartitionKey()
            => "AssetCategory";

        private static string GetRowKey(string id)
            => id;
    }
}
