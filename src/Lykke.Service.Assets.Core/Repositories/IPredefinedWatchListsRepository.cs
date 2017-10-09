using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IPredefinedWatchListsRepository
    {
        Task<IEnumerable<IWatchList>> GetAllAsync();

        Task<IWatchList> GetAsync(string watchListId);

        Task RemoveAsync(string watchListId);

        Task UpsertAsync(IWatchList watchList);
    }
}