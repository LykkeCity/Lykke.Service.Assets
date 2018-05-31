using System;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Client.Cache
{
    /// <summary>
    /// Expiring dictionary cache where the cache entry expires after given time.
    /// </summary>
    /// <typeparam name="T">the type of cached item</typeparam>
    internal sealed class ExpiringDictionaryCache<T> : DictionaryCache<T>
        where T : ICacheItem
    {
        /// <summary>
        /// Create a new expiring dictionary cache.
        /// </summary>
        /// <param name="expirationTime">expiration time</param>
        /// <param name="updater">item updater</param>
        public ExpiringDictionaryCache(TimeSpan expirationTime, IUpdater<T> updater)
            : base(updater, expirationTime)
        {
        }
    }
}
