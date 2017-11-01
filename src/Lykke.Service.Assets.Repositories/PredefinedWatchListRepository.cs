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
    public class PredefinedWatchListRepository : IPredefinedWatchListRepository
    {
        private readonly INoSQLTableStorage<PredefinedWatchListEntity> _predefinedWatchListTable;


        public PredefinedWatchListRepository(
            INoSQLTableStorage<PredefinedWatchListEntity> predefinedWatchListTable)
        {
            _predefinedWatchListTable = predefinedWatchListTable;
        }
        
        public async Task<IEnumerable<IWatchList>> GetAllAsync()
        {
            var entities = await _predefinedWatchListTable.GetDataAsync(GetPartitionKey());

            return entities.Select(Mapper.Map<WatchListDto>);
        }

        public async Task<IWatchList> GetAsync(string watchListId)
        {
            var entity = await _predefinedWatchListTable.GetDataAsync(GetPartitionKey(), GetRowKey(watchListId));

            return Mapper.Map<WatchListDto>(entity);
        }

        public async Task RemoveAsync(string watchListId)
        {
            await _predefinedWatchListTable.DeleteIfExistAsync(GetPartitionKey(), GetRowKey(watchListId));
        }

        public async Task UpsertAsync(IWatchList watchList)
        {
            var entity = Mapper.Map<PredefinedWatchListEntity>(watchList);

            entity.PartitionKey = GetPartitionKey();
            entity.RowKey       = GetRowKey(watchList.Id);

            await _predefinedWatchListTable.InsertOrReplaceAsync(entity);
        }

        private static string GetPartitionKey()
            => "PublicWatchList";

        private static string GetRowKey(string watchListId)
            => watchListId;
    }
}