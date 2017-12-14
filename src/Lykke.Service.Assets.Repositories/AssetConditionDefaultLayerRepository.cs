using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionDefaultLayerRepository : IAssetConditionDefaultLayerRepository
    {
        private readonly INoSQLTableStorage<AssetConditionDefaultLayerEntity> _tableStorage;

        public AssetConditionDefaultLayerRepository(INoSQLTableStorage<AssetConditionDefaultLayerEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public async Task<IAssetConditionDefaultLayer> GetAsync()
        {
            return await _tableStorage.GetDataAsync(GetPartitionKey(), GetRowKey());
        }

        public async Task InsertOrUpdate(IAssetConditionDefaultLayer settings)
        {
            var entity = new AssetConditionDefaultLayerEntity
            {
                PartitionKey = GetPartitionKey(),
                RowKey = GetRowKey()
            };

            Mapper.Map(settings, entity);

            await _tableStorage.InsertOrReplaceAsync(entity);
        }

        private static string GetPartitionKey()
            => "ConditionLayerDefault";

        private static string GetRowKey()
            => "Settings";
    }
}
