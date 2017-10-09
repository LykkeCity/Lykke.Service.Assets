using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Repositories.Entities;

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
            throw new System.NotImplementedException();
        }

        public async Task<IWatchList> GetAsync(string userId, string watchListId)
        {
            throw new System.NotImplementedException();
        }

        public async Task RemoveAsync(string userId, string watchListId)
        {
            throw new System.NotImplementedException();
        }

        public async Task UpsertAsync(string userId, IWatchList watchList)
        {
            var entity = Mapper.Map<CustomWatchListEntity>(watchList);

            entity.PartitionKey = GetPartitionKey(userId);
            entity.RowKey       = GetRowKey(watchList.Id);
            entity.UserId       = userId;

            await _customWatchListTable.InsertOrReplaceAsync(entity);
        }

        private static string GetPartitionKey(string userId)
            => userId;

        private static string GetRowKey(string watchListId)
            => watchListId;
    }
}