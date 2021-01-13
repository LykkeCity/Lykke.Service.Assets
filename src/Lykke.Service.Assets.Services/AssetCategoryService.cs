using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using Lykke.Common.Log;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.NoSql.Models;

namespace Lykke.Service.Assets.Services
{
    public class AssetCategoryService : IAssetCategoryService, IStartable
    {
        private readonly IAssetCategoryRepository _assetCategoryRepository;
        private readonly IMyNoSqlWriterWrapper<AssetCategoryNoSql> _myNoSqlWriter;

        public AssetCategoryService(
            IAssetCategoryRepository assetCategoryRepository,
            IMyNoSqlWriterWrapper<AssetCategoryNoSql> myNoSqlWriter,
            ILogFactory logFactory)
        {
            _assetCategoryRepository = assetCategoryRepository;
            _myNoSqlWriter = myNoSqlWriter;
        }


        public async Task<IAssetCategory> AddAsync(IAssetCategory assetCategory)
        {
            await _assetCategoryRepository.AddAsync(assetCategory);
            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetCategoryNoSql.Create(assetCategory));

            return assetCategory;
        }

        public async Task<IAssetCategory> GetAsync(string id)
        {
            return await _assetCategoryRepository.GetAsync(id);
        }

        public async Task<IEnumerable<IAssetCategory>> GetAllAsync()
        {
            return await _assetCategoryRepository.GetAllAsync();
        }

        public async Task RemoveAsync(string id)
        {
            await _assetCategoryRepository.RemoveAsync(id);
            await _myNoSqlWriter.TryDeleteAsync(AssetCategoryNoSql.GeneratePartitionKey(), AssetCategoryNoSql.GenerateRowKey(id));
        }

        public async Task UpdateAsync(IAssetCategory assetCategory)
        {
            await _assetCategoryRepository.UpdateAsync(assetCategory);
            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetCategoryNoSql.Create(assetCategory));
        }

        public void Start()
        {
            _myNoSqlWriter.Start(ReadAllData);
        }

        private IList<AssetCategoryNoSql> ReadAllData()
        {
            return GetAllAsync().GetAwaiter().GetResult().Select(AssetCategoryNoSql.Create).ToList();
        }
    }
}
