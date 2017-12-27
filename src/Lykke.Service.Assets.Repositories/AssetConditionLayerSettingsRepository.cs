using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionLayerSettingsRepository : IAssetConditionLayerSettingsRepository
    {
        private readonly INoSQLTableStorage<AssetConditionLayerSettingsEntity> _storage;

        public AssetConditionLayerSettingsRepository(INoSQLTableStorage<AssetConditionLayerSettingsEntity> storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Returns default asset condition layer settings.
        /// </summary>
        /// <returns>The default asset condition layer settings.</returns>
        public async Task<IAssetConditionLayerSettings> GetAsync()
        {
            AssetConditionLayerSettingsEntity entity = await _storage.GetDataAsync(GetPartitionKey(), GetRowKey());

            var result = Mapper.Map<AssetConditionLayerSettingsDto>(entity);

            return result ?? new AssetConditionLayerSettingsDto();
        }

        /// <summary>
        /// Updates default asset conditions layer settings.
        /// </summary>
        /// <param name="settings">The asset conditon layer settings.</param>
        /// <returns></returns>
        public async Task UpdateAsync(IAssetConditionLayerSettings settings)
        {
            var entity = new AssetConditionLayerSettingsEntity(GetPartitionKey(), GetRowKey());

            Mapper.Map(entity, settings);

            await _storage.InsertOrReplaceAsync(entity);
        }
        
        private static string GetPartitionKey()
            => "ConditionLayerSettings";

        private static string GetRowKey()
            => "Default";
    }
}
