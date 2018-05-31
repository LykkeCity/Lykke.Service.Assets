using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Cache;

namespace Lykke.Service.Assets.Client.Updaters
{
    /// <summary>
    /// Updater of <typeparamref name="T"/> for retrieving the contents of a dictionary cache.
    /// </summary>
    /// <typeparam name="T">the type of cache entry to retrieve</typeparam>
    public interface IUpdater<T>
        where T : ICacheItem
    {
        /// <summary>
        /// Retrieves the cache items.
        /// </summary>
        Task<IEnumerable<T>> GetItemsAsync(CancellationToken token);
    }
}
