using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Autofac;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.NoSql.Models;


namespace Lykke.Service.Assets.Services
{
    public class WatchListService : IWatchListService, IStartable
    {
        private readonly ICustomWatchListRepository     _customWatchListRepository;
        private readonly IPredefinedWatchListRepository _predefinedWatchListRepository;
        private readonly IMyNoSqlWriterWrapper<WatchListCustomNoSql> _myNoSqlWriterCustom;
        private readonly IMyNoSqlWriterWrapper<WatchListPredefinedNoSql> _myNoSqlWriterPredefined;
        private readonly int _maxClientsInNoSqlCache;
        private ILog _log;

        public WatchListService(
            ICustomWatchListRepository customWatchListRepository,
            IPredefinedWatchListRepository predefinedWatchListRepository, 
            IMyNoSqlWriterWrapper<WatchListCustomNoSql> myNoSqlWriterCustom,
            ILogFactory logFactory,
            IMyNoSqlWriterWrapper<WatchListPredefinedNoSql> myNoSqlWriterPredefined,
            int maxClientsInNoSqlCache)
        {
            _log = logFactory.CreateLog(this);
            _customWatchListRepository     = customWatchListRepository;
            _predefinedWatchListRepository = predefinedWatchListRepository;
            _myNoSqlWriterCustom = myNoSqlWriterCustom;
            _maxClientsInNoSqlCache = maxClientsInNoSqlCache;
            _myNoSqlWriterPredefined = myNoSqlWriterPredefined;
        }


        public async Task<IWatchList> AddCustomAsync(string userId, IWatchList watchList)
        {
            await _customWatchListRepository.UpsertAsync(userId, watchList);
            await ReLoadCustomWatchListToNoSqlCache(userId);

            return watchList;
        }

        public async Task<IWatchList> AddPredefinedAsync(IWatchList watchList)
        {
            await _predefinedWatchListRepository.UpsertAsync(watchList);
            await _myNoSqlWriterPredefined.TryInsertOrReplaceAsync(WatchListPredefinedNoSql.Create(watchList));


            return watchList;
        }
        
        public async Task<IEnumerable<IWatchList>> GetAllAsync(string userId)
        {
            var watchLists = await Task.WhenAll
            (
                GetAllCustomAsync(userId),
                GetAllPredefinedAsync()
            );

            var customWatchLists     = watchLists[0].ToArray();
            var predefinedWatchLists = watchLists[1].ToArray();
            var allWatchListsForUser = new List<IWatchList>();

            var allAssetsWatchList = predefinedWatchLists.FirstOrDefault(IsAllAssetsWatchList);
            if (allAssetsWatchList != null)
            {
                allWatchListsForUser.Add(allAssetsWatchList);
            }

            allWatchListsForUser.AddRange
            (
                customWatchLists
                    .OrderBy(x => x.Order)
                    .ThenBy(x => x.Name)
            );

            allWatchListsForUser.AddRange
            (
                predefinedWatchLists
                    .Where(x => !IsAllAssetsWatchList(x))
                    .OrderBy(x => x.Order)
                    .ThenBy(x => x.Name)
            );


            return allWatchListsForUser
                .GroupBy(x => x.Id)
                .Select(x => x.First());
        }

        public async Task<IEnumerable<IWatchList>> GetAllCustomAsync(string userId)
        {
            var data = await ReLoadCustomWatchListToNoSqlCache(userId);
            return data;
        }

        public async Task<IEnumerable<IWatchList>> GetAllPredefinedAsync()
        {
            return await _predefinedWatchListRepository.GetAllAsync();
        }

        public async Task<IWatchList> GetCustomAsync(string userId, string watchListId)
        {
            await ReLoadCustomWatchListToNoSqlCache(userId);
            return await _customWatchListRepository.GetAsync(userId, watchListId);
        }

        public async Task<IWatchList> GetPredefinedAsync(string watchListId)
        {
            return await _predefinedWatchListRepository.GetAsync(watchListId);
        }

        public async Task RemoveCustomAsync(string userId, string watchListId)
        {
            await _customWatchListRepository.RemoveAsync(userId, watchListId);
            await ReLoadCustomWatchListToNoSqlCache(userId);
        }

        public async Task RemovePredefinedAsync(string watchListId)
        {
            await _predefinedWatchListRepository.RemoveAsync(watchListId);
            await _myNoSqlWriterPredefined.TryDeleteAsync(WatchListPredefinedNoSql.GeneratePartitionKey(), WatchListPredefinedNoSql.GenerateRowKey(watchListId));
        }

        public async Task UpdateCustomAsync(string userId, IWatchList watchList)
        {
            await _customWatchListRepository.UpsertAsync(userId, watchList);
            await ReLoadCustomWatchListToNoSqlCache(userId);
        }

        public async Task UpdatePredefinedAsync(IWatchList watchList)
        {
            await _predefinedWatchListRepository.UpsertAsync(watchList);
            await _myNoSqlWriterPredefined.TryInsertOrReplaceAsync(WatchListPredefinedNoSql.Create(watchList));
        }

        private static bool IsAllAssetsWatchList(IWatchList watchList)
        {
            // Legacy. Probably, we should use settings to detect all assets watch list.
            return watchList.Name.Equals(Constants.AllAssetsWatchListName, StringComparison.InvariantCultureIgnoreCase);
        }

        public void Start()
        {
            _myNoSqlWriterCustom.StartWithClearing(_maxClientsInNoSqlCache);
            _myNoSqlWriterPredefined.Start(ReadAllPredefined);
        }

        private IList<WatchListPredefinedNoSql> ReadAllPredefined()
        {
            var data = _predefinedWatchListRepository.GetAllAsync().GetAwaiter().GetResult();
            return data.Select(WatchListPredefinedNoSql.Create).ToList();
        }

        private async Task<IEnumerable<IWatchList>> ReLoadCustomWatchListToNoSqlCache(string clientId)
        {
            try
            {
                var data = await _customWatchListRepository.GetAllAsync(clientId);
                var list = data.Select(e => WatchListCustomNoSql.Create(clientId, e)).ToList();
                await _myNoSqlWriterCustom.CleanAndBulkInsertAsync(WatchListCustomNoSql.GeneratePartitionKey(clientId),
                    list);

                return list;
            }
            catch(Exception ex)
            {
                _log.Error(ex, $"Cannot execute CleanAndBulkInsertAsync in NoSql. ClientId: {clientId}");
                throw;
            }
        }
    }
}
