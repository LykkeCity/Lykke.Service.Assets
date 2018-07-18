using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Cache
{
    /// <summary>
    /// Simple in-memory client side cache.
    /// </summary>
    internal interface IDictionaryCache<T>
        where T : ICacheItem
    {
        /// <summary>
        /// Resets the cache.
        /// </summary>
        Task Reset(CancellationToken token);

        /// <summary>
        /// Update a cache item
        /// </summary>
        /// <param name="id"></param>
        /// <param name="item"></param>
        /// <param name="token"></param>
        /// <returns></returns>
        Task Update(string id, T item, CancellationToken token);

        /// <summary>
        /// Try to get cached item with given id.
        /// </summary>
        Task<T> TryGet(string id, CancellationToken token);

        /// <summary>
        /// Get all cached items.
        /// </summary>
        Task<IReadOnlyCollection<T>> GetAll(CancellationToken token);
    }
}
