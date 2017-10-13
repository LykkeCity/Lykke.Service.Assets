using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetSettingsRepository : IAssetSettingsRepository
    {
        private readonly INoSQLTableStorage<AssetSettingsEntity> _assetSettingsTable;


        public AssetSettingsRepository(
            INoSQLTableStorage<AssetSettingsEntity> assetSettingsTable)
        {
            _assetSettingsTable = assetSettingsTable;
        }


        public async Task<IEnumerable<IAssetSettings>> GetAllAsync()
        {
            return await _assetSettingsTable.GetDataAsync(GetPartitionKey());
        }

        public async Task<IAssetSettings> GetAsync(string asset)
        {
            return await _assetSettingsTable.GetDataAsync(GetPartitionKey(), GetRowKey(asset));
        }

        public async Task RemoveAsync(string asset)
        {
            await _assetSettingsTable.DeleteAsync(GetPartitionKey(), GetRowKey(asset));
        }

        public async Task UpsertAsync(IAssetSettings settings)
        {
            var entity = Mapper.Map<AssetSettingsEntity>(settings);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(settings.Asset);

            await _assetSettingsTable.InsertOrReplaceAsync(entity);
        }

        private static string GetPartitionKey()
            => "Asset";

        private static string GetRowKey(string asset)
            => asset;
    }
}