using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Common.Log;

namespace Lykke.Service.Assets.Client.Cache
{
    /// <summary>
    /// Simple in-memory client side cache.
    /// </summary>
    public interface IDictionaryCache<T>
        where T : ICacheItem
    {
        /// <summary>
        /// Starts an automatic updater that keeps the cache updated on a background thread.
        /// </summary>
        IDisposable StartAutoUpdate(string componentName, ILog log, Func<Task<IEnumerable<T>>> getAllAsync);

        /// <summary>
        /// Update the cache when cache has expired.
        /// </summary>
        Task EnsureCacheIsUpdatedAsync(Func<Task<IEnumerable<T>>> getAllAsync);

        /// <summary>
        /// Update the cache with given data.
        /// </summary>
        void Update(IEnumerable<T> items);

        /// <summary>
        /// Try to get cached item with given id.
        /// </summary>
        T TryGet(string id);

        /// <summary>
        /// Get all cached items.
        /// </summary>
        IReadOnlyCollection<T> GetAll();
    }
}
