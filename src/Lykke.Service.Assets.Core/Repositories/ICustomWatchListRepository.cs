using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface ICustomWatchListRepository
    {
        Task<IEnumerable<IWatchList>> GetAllAsync(string userId);

        Task<IWatchList> GetAsync(string userId, string watchListId);

        Task RemoveAsync(string userId, string watchListId);

        Task UpsertAsync(string userId, IWatchList watchList);
    }
}