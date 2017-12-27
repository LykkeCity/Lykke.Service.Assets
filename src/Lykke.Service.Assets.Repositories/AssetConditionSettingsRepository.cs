using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionSettingsRepository : IAssetConditionSettingsRepository
    {
        private readonly INoSQLTableStorage<AssetConditionSettingsEntity> _storage;

        public AssetConditionSettingsRepository(INoSQLTableStorage<AssetConditionSettingsEntity> storage)
        {
            _storage = storage;
        }

        /// <summary>
        /// Returns default asset conditions settings.
        /// </summary>
        /// <returns>The default asset conditions settings.</returns>
        public async Task<IAssetConditionSettings> GetAsync()
        {
            AssetConditionSettingsEntity entity = await _storage.GetDataAsync(GetPartitionKey(), GetRowKey());

            var result = Mapper.Map<AssetConditionSettingsDto>(entity);

            return result ?? new AssetConditionSettingsDto();
        }

        /// <summary>
        /// Updates default asset conditions settings.
        /// </summary>
        /// <param name="settings">The default asset conditions settings.</param>
        /// <returns></returns>
        public async Task UpdateAsync(IAssetConditionSettings settings)
        {
            var entity = new AssetConditionSettingsEntity(GetPartitionKey(), GetRowKey());

            Mapper.Map(settings, entity);

            await _storage.InsertOrReplaceAsync(entity);
        }

        private static string GetPartitionKey()
            => "ConditionDefault";

        private static string GetRowKey()
            => "Default";
    }
}
