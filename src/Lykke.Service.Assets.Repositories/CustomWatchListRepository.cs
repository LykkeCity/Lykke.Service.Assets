using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.Service.Assets.Repositories.DTOs;
using System.Linq;

namespace Lykke.Service.Assets.Repositories
{
    public class CustomWatchListRepository : ICustomWatchListRepository
    {
        private readonly INoSQLTableStorage<CustomWatchListEntity> _customWatchListTable;


        public CustomWatchListRepository(
            INoSQLTableStorage<CustomWatchListEntity> customWatchListTable)
        {
            _customWatchListTable = customWatchListTable;
        }


        public async Task<IEnumerable<IWatchList>> GetAllAsync(string userId)
        {
            var entities = await _customWatchListTable.GetDataAsync(GetPartitionKey(userId));

            return entities.Select(Mapper.Map<WatchListDto>);
        }

        public async Task<IWatchList> GetAsync(string userId, string watchListId)
        {
            var entity = await _customWatchListTable.GetDataAsync(GetPartitionKey(userId), GetRowKey(watchListId));

            return Mapper.Map<WatchListDto>(entity);
        }

        public async Task RemoveAsync(string userId, string watchListId)
        {
            await _customWatchListTable.DeleteIfExistAsync(GetPartitionKey(userId), GetRowKey(watchListId));
        }

        public async Task UpsertAsync(string userId, IWatchList watchList)
        {
            var entity = Mapper.Map<CustomWatchListEntity>(watchList);

            entity.PartitionKey = GetPartitionKey(userId);
            entity.RowKey       = GetRowKey(watchList.Id);

            await _customWatchListTable.InsertOrReplaceAsync(entity);
        }

        private static string GetPartitionKey(string userId)
            => userId;

        private static string GetRowKey(string watchListId)
            => watchListId;
    }
}