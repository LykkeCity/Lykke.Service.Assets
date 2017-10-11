using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IWatchListService
    {
        Task<IWatchList> AddCustomAsync(string userId, IWatchList watchList);

        Task<IWatchList> AddPredefinedAsync(IWatchList watchList);

        Task<IEnumerable<IWatchList>> GetAllAsync(string userId);

        Task<IEnumerable<IWatchList>> GetAllCustomAsync(string userId);

        Task<IEnumerable<IWatchList>> GetAllPredefinedAsync();

        Task<IWatchList> GetCustomAsync(string userId, string watchListId);

        Task<IWatchList> GetPredefinedAsync(string watchListId);

        Task RemoveCustomAsync(string userId, string watchListId);

        Task RemovePredefinedAsync(string watchListId);

        Task UpdateCustomAsync(string userId, IWatchList watchList);

        Task UpdatePredefinedAsync(IWatchList watchList);
    }
}