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
    public class AssetRepository : IAssetRepository
    {
        private readonly INoSQLTableStorage<AssetEntity> _assetTable;


        public AssetRepository(INoSQLTableStorage<AssetEntity> assetTable)
        {
            _assetTable = assetTable;
        }

        public async Task AddAsync(IAsset asset)
        {
            var entity = Mapper.Map<AssetEntity>(asset);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(asset.Id);

            await _assetTable.InsertAsync(entity);
        }

        public async Task<IEnumerable<IAsset>> GetAllAsync()
        {
            return (await _assetTable.GetDataAsync(GetPartitionKey()))
                .Select(Mapper.Map<AssetDto>);
        }

        public async Task<IAsset> GetAsync(string id)
        {
            var asset = await _assetTable.GetDataAsync(GetPartitionKey(), GetRowKey(id));

            return Mapper.Map<AssetDto>(asset);
        }

        public async Task<IEnumerable<IAsset>> GetAsync(string[] ids)
        {
            return (await _assetTable.GetDataAsync(GetPartitionKey(), ids.Select(GetRowKey)))
                .Select(Mapper.Map<AssetDto>);
        }

        public async Task RemoveAsync(string id)
        {
            await _assetTable.DeleteAsync(GetPartitionKey(), GetRowKey(id));
        }

        public async Task UpdateAsync(IAsset asset)
        {
            await _assetTable.MergeAsync(GetPartitionKey(), GetRowKey(asset.Id), x =>
            {
                Mapper.Map(asset, x);

                return x;
            });
        }

        public static string GetPartitionKey()
            => "Asset";

        public static string GetRowKey(string id)
            => id;
    }
}