using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetDefaultConditionRepository : IAssetDefaultConditionRepository
    {
        private readonly INoSQLTableStorage<AssetDefaultConditionEntity> _storage;

        public AssetDefaultConditionRepository(INoSQLTableStorage<AssetDefaultConditionEntity> storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Returns default asset conditions.
        /// </summary>
        /// <returns>The default asset conditions.</returns>
        public async Task<IAssetDefaultCondition> GetAsync(string layerId)
        {
            AssetDefaultConditionEntity entity = await _storage.GetDataAsync(GetPartitionKey(), GetRowKey(layerId));

            return Mapper.Map<AssetDefaultConditionDto>(entity);
        }

        /// <summary>
        /// Inserts a default asset conditions.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="assetDefaultCondition">The default asset conditons.</param>
        public async Task InsertAsync(string layerId, IAssetDefaultCondition assetDefaultCondition)
        {
            var entity = new AssetDefaultConditionEntity(GetPartitionKey(), GetRowKey(layerId), layerId);

            Mapper.Map(assetDefaultCondition, entity);

            await _storage.InsertAsync(entity);
        }

        /// <summary>
        /// Updates default asset conditions.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <param name="assetDefaultCondition">The default asset conditions.</param>
        public async Task UpdateAsync(string layerId, IAssetDefaultCondition assetDefaultCondition)
        {
            await _storage.MergeAsync(GetPartitionKey(), GetRowKey(layerId), entity =>
            {
                Mapper.Map(assetDefaultCondition, entity);
                return entity;
            });
        }

        /// <summary>
        /// Deletes default asset conditons associated with layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        public async Task DeleteAsync(string layerId)
        {
            await _storage.DeleteAsync(GetPartitionKey(), GetRowKey(layerId));
        }

        private static string GetPartitionKey()
            => "DefaultCondition";

        private static string GetRowKey(string layerId)
            => layerId;
    }
}
