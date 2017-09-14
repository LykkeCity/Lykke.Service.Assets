using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetCategoryRepository : IAssetCategoryRepository, IDictionaryRepository<IAssetCategory>
    {
        private readonly INoSQLTableStorage<AssetCategoryEntity> _table;

        public AssetCategoryRepository(INoSQLTableStorage<AssetCategoryEntity> table)
        {
            _table = table;
        }

        public Task InsertAssetCategory(IAssetCategory assetCategory)
        {
            return _table.InsertAsync(AssetCategoryEntity.Create(assetCategory));
        }

        public Task DeleteAssetCategory(string assetCategoryId)
        {
            return _table.DeleteAsync(AssetCategoryEntity.Partition, assetCategoryId);
        }

        public async Task UpdateAssetCategory(IAssetCategory assetCategory)
        {
            await _table.ReplaceAsync(AssetCategoryEntity.Partition, assetCategory.Id, entity =>
            {
                entity.Name = assetCategory.Name;
                entity.IosIconUrl = assetCategory.IosIconUrl;
                entity.AndroidIconUrl = assetCategory.AndroidIconUrl;
                entity.SortOrder = assetCategory.SortOrder;
                return entity;
            });
        }

        public async Task<IAssetCategory> GetAssetCategory(string id)
        {
            return await _table.GetDataAsync(AssetCategoryEntity.Partition, id);
        }

        public async Task<IEnumerable<IAssetCategory>> GetAllAsync()
        {
            return await _table.GetDataAsync();
        }
    }
}
