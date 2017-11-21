using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;


namespace Lykke.Service.Assets.Services
{
    public class WatchListService : IWatchListService
    {
        private readonly ICustomWatchListRepository     _customWatchListRepository;
        private readonly IPredefinedWatchListRepository _predefinedWatchListRepository;

        public WatchListService(
            ICustomWatchListRepository customWatchListRepository,
            IPredefinedWatchListRepository predefinedWatchListRepository)
        {
            _customWatchListRepository     = customWatchListRepository;
            _predefinedWatchListRepository = predefinedWatchListRepository;
        }


        public async Task<IWatchList> AddCustomAsync(string userId, IWatchList watchList)
        {
            await _customWatchListRepository.UpsertAsync(userId, watchList);

            return watchList;
        }

        public async Task<IWatchList> AddPredefinedAsync(IWatchList watchList)
        {
            await _predefinedWatchListRepository.UpsertAsync(watchList);

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
            var allWatchListsForUser = new List<IWatchList>
            {
                predefinedWatchLists
                    .FirstOrDefault(IsAllAssetsWatchList)
            };


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
                .Where(x => x != null)
                .GroupBy(x => x.Id)
                .Select(x => x.First());
        }

        public async Task<IEnumerable<IWatchList>> GetAllCustomAsync(string userId)
        {
            return await _customWatchListRepository.GetAllAsync(userId);
        }

        public async Task<IEnumerable<IWatchList>> GetAllPredefinedAsync()
        {
            return await _predefinedWatchListRepository.GetAllAsync();
        }

        public async Task<IWatchList> GetCustomAsync(string userId, string watchListId)
        {
            return await _customWatchListRepository.GetAsync(userId, watchListId);
        }

        public async Task<IWatchList> GetPredefinedAsync(string watchListId)
        {
            return await _predefinedWatchListRepository.GetAsync(watchListId);
        }

        public async Task RemoveCustomAsync(string userId, string watchListId)
        {
            await _customWatchListRepository.RemoveAsync(userId, watchListId);
        }

        public async Task RemovePredefinedAsync(string watchListId)
        {
            await _predefinedWatchListRepository.RemoveAsync(watchListId);
        }

        public async Task UpdateCustomAsync(string userId, IWatchList watchList)
        {
            await _customWatchListRepository.UpsertAsync(userId, watchList);
        }

        public async Task UpdatePredefinedAsync(IWatchList watchList)
        {
            await _predefinedWatchListRepository.UpsertAsync(watchList);
        }

        private static bool IsAllAssetsWatchList(IWatchList watchList)
        {
            // Legacy. Probably, we should use settings to detect all assets watch list.
            return watchList.Name.Equals("All assets", StringComparison.InvariantCultureIgnoreCase);
        }
    }
}
