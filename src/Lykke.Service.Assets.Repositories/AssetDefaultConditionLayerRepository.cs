using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetDefaultConditionLayerRepository : IAssetDefaultConditionLayerRepository
    {
        private readonly INoSQLTableStorage<AssetDefaultConditionLayerEntity> _storage;

        public AssetDefaultConditionLayerRepository(INoSQLTableStorage<AssetDefaultConditionLayerEntity> storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Returns default asset condition layer.
        /// </summary>
        /// <returns>The default asset condition layer.</returns>
        public async Task<IAssetDefaultConditionLayer> GetAsync()
        {
            AssetDefaultConditionLayerEntity entity = await _storage.GetDataAsync(GetPartitionKey(), GetRowKey());

            var result = Mapper.Map<AssetDefaultConditionLayerDto>(entity);

            return result ?? new AssetDefaultConditionLayerDto
            {
                Id = GetRowKey()
            };
        }

        /// <summary>
        /// Updates default asset conditions layer.
        /// </summary>
        /// <param name="settings">The asset conditions layer settings.</param>
        /// <returns></returns>
        public async Task UpdateAsync(IAssetConditionLayerSettings settings)
        {
            var entity = new AssetDefaultConditionLayerEntity(GetPartitionKey(), GetRowKey());

            Mapper.Map(settings, entity);

            await _storage.InsertOrReplaceAsync(entity);
        }
        
        private static string GetPartitionKey()
            => "DefaultConditionLayer";

        private static string GetRowKey()
            => "DefaultLayer";
    }
}
