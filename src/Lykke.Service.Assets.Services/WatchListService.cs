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
        private readonly ICustomWatchListRepository      _customWatchListRepository;
        private readonly IPredefinedWatchListsRepository _predefinedWatchListsRepository;

        public WatchListService(
            ICustomWatchListRepository customWatchListRepository,
            IPredefinedWatchListsRepository predefinedWatchListsRepository)
        {
            _customWatchListRepository      = customWatchListRepository;
            _predefinedWatchListsRepository = predefinedWatchListsRepository;
        }


        public async Task<IWatchList> AddCustomAsync(string userId, IWatchList watchList)
        {
            await _customWatchListRepository.UpsertAsync(userId, watchList);

            return watchList;
        }

        public async Task<IWatchList> AddPredefinedAsync(IWatchList watchList)
        {
            await _predefinedWatchListsRepository.UpsertAsync(watchList);

            return watchList;
        }
        
        public async Task<IEnumerable<IWatchList>> GetAllAsync(string userId)
        {
            var customWatchLists     = await GetAllCustomAsync(userId);
            var predefinedWatchLists = await GetAllPredefinedAsync();

            return customWatchLists.Concat(predefinedWatchLists);
        }

        public async Task<IEnumerable<IWatchList>> GetAllCustomAsync(string userId)
        {
            return await _customWatchListRepository.GetAllAsync(userId);
        }

        public async Task<IEnumerable<IWatchList>> GetAllPredefinedAsync()
        {
            return await _predefinedWatchListsRepository.GetAllAsync();
        }

        public async Task<IWatchList> GetCustomAsync(string userId, string watchListId)
        {
            return await _customWatchListRepository.GetAsync(userId, watchListId);
        }

        public async Task<IWatchList> GetPredefinedAsync(string watchListId)
        {
            return await _predefinedWatchListsRepository.GetAsync(watchListId);
        }

        public async Task RemoveCustomAsync(string userId, string watchListId)
        {
            await _customWatchListRepository.RemoveAsync(userId, watchListId);
        }

        public async Task RemovePredefinedAsync(string watchListId)
        {
            await _predefinedWatchListsRepository.RemoveAsync(watchListId);
        }

        public async Task UpdateCustomAsync(string userId, IWatchList watchList)
        {
            await _customWatchListRepository.UpsertAsync(userId, watchList);
        }

        public async Task UpdatePredefinedAsync(IWatchList watchList)
        {
            await _predefinedWatchListsRepository.UpsertAsync(watchList);
        }
    }
}