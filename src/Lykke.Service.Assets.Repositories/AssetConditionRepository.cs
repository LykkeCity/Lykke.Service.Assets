using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionRepository : IAssetConditionRepository
    {
        private readonly INoSQLTableStorage<AssetConditionEntity> _storage;

        public AssetConditionRepository(INoSQLTableStorage<AssetConditionEntity> storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Returns asset conditions by layer id.
        /// </summary>
        /// <param name="layerId">The asset condition layer id.</param>
        /// <returns>A collection of asset conditions.</returns>
        public async Task<IEnumerable<IAssetCondition>> GetAsync(string layerId)
        {
            return await _storage.GetDataAsync(GetPartitionKey(layerId));
        }

        /// <summary>
        /// Returns asset conditions for each layer.
        /// </summary>
        /// <param name="layers">The collection of layers id.</param>
        /// <returns>A collection of asset conditions.</returns>
        public async Task<IEnumerable<IAssetCondition>> GetAsync(IEnumerable<string> layers)
        {
            return await _storage.GetDataAsync(layers.Select(GetPartitionKey));
        }

        /// <summary>
        /// Adds or entirely replaces an asset condition.
        /// </summary>
        /// <param name="layerId">The id of the layer that contains asset condition.</param>
        /// <param name="assetCondition">The asset conditons.</param>
        /// <returns></returns>
        public async Task InsertOrReplaceAsync(string layerId, IAssetCondition assetCondition)
        {
            var entity = new AssetConditionEntity(GetPartitionKey(layerId), GetRowKey(assetCondition.Asset), layerId);

            Mapper.Map(assetCondition, entity);
            
            await _storage.InsertOrReplaceAsync(entity);
        }

        /// <summary>
        /// Deletes all asset conditons associated with layer.
        /// </summary>
        /// <param name="layerId">The id of the layer that contains asset condition.</param>
        /// <returns></returns>
        public async Task DeleteAsync(string layerId)
        {
            var entities = await _storage.GetDataAsync(GetPartitionKey(layerId));

            await _storage.DeleteAsync(entities);
        }

        /// <summary>
        /// Deletes an asset conditons.
        /// </summary>
        /// <param name="layerId">The id of the layer that contains asset condition.</param>
        /// <param name="asset">The id of asset associated with asset condition.</param>
        /// <returns></returns>
        public async Task DeleteAsync(string layerId, string asset)
        {
            await _storage.DeleteAsync(GetPartitionKey(layerId), GetRowKey(asset));
        }
        
        private static string GetPartitionKey(string layerId)
            => $"Condition_{layerId}";

        private static string GetRowKey(string asset)
            => asset;
    }
}
