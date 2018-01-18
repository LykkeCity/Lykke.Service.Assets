using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionLayerRepository : IAssetConditionLayerRepository
    {
        private readonly INoSQLTableStorage<AssetConditionLayerEntity> _storage;

        public AssetConditionLayerRepository(INoSQLTableStorage<AssetConditionLayerEntity> storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Returns all asset conditions layers.
        /// </summary>
        /// <returns>A collection of conditions layers</returns>
        public async Task<IEnumerable<IAssetConditionLayer>> GetAsync()
        {
            IEnumerable<AssetConditionLayerEntity> layers =
                await _storage.GetDataAsync(GetPartitionKey());

            var result = Mapper.Map<List<AssetConditionLayerDto>>(layers);

            return result;
        }

        /// <summary>
        /// Returns asset conditions layer.
        /// </summary>
        /// <param name="layerId">The asset conditions layer id.</param>
        /// <returns>An asset conditions layer</returns>
        public async Task<IAssetConditionLayer> GetAsync(string layerId)
        {
            AssetConditionLayerEntity layer = await _storage.GetDataAsync(GetPartitionKey(), GetRowKey(layerId));

            var result = Mapper.Map<AssetConditionLayerDto>(layer);

            return result;
        }

        /// <summary>
        /// Returns asset conditions layers.
        /// </summary>
        /// <param name="layerIds">The collection of layers id.</param>
        /// <returns>A collection of asset conditions layers</returns>
        public async Task<IEnumerable<IAssetConditionLayer>> GetAsync(IEnumerable<string> layerIds)
        {
            IEnumerable<AssetConditionLayerEntity> layers =
                await _storage.GetDataAsync(GetPartitionKey(), layerIds.Select(GetRowKey));

            var result = Mapper.Map<List<AssetConditionLayerDto>>(layers);

            return result;
        }

        /// <summary>
        /// Adds or entirely replaces an asset conditions layer.
        /// </summary>
        /// <param name="layer">The asset conditon layer.</param>
        /// <returns></returns>
        public async Task InsertOrReplaceAsync(IAssetConditionLayer layer)
        {
            var entity = new AssetConditionLayerEntity(GetPartitionKey(), GetRowKey(layer.Id));

            Mapper.Map(layer, entity);

            await _storage.InsertOrReplaceAsync(entity);
        }

        /// <summary>
        /// Deletes an asset conditon layer.
        /// </summary>
        /// <param name="layerId">The layer id.</param>
        /// <returns></returns>
        public async Task DeleteAsync(string layerId)
        {
            await _storage.DeleteAsync(GetPartitionKey(), GetRowKey(layerId));
        }

        private static string GetPartitionKey()
            => "ConditionLayer";

        private static string GetRowKey(string layerId)
            => layerId.Trim().ToLower();
    }
}
