using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IWatchListsRepository
    {
        Task<IWatchList> UpsertPredefinedAsync(IWatchList model);

        Task<IEnumerable<IWatchList>> GetPredefinedAsync();

        Task<IWatchList> GetPredefinedAsync(string id);

        Task DeletePredefinedAsync(string id);

        Task<IWatchList> UpsertCustomAsync(string userId, IWatchList model);

        Task<IEnumerable<IWatchList>> GetCustomAsync(string userId);

        Task<IWatchList> GetCustomAsync(string userId, string id);

        Task DeleteCustomAsync(string userId, string id);

        Task<IEnumerable<IWatchList>> GetAllAsync(string userId);
    }
}