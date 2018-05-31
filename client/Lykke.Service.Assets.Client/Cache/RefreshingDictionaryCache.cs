using System;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Client.Cache
{
    /// <summary>
    /// A dictionary cache that refreshes/synchronizes in the background.
    /// </summary>
    /// <typeparam name="T">the type of cached item</typeparam>
    internal sealed class RefreshingDictionaryCache<T> : DictionaryCache<T>, IDisposable
        where T : ICacheItem
    {
        private readonly TimerTrigger _trigger;

        /// <summary>
        /// Creates a new refreshing dictionary cache.
        /// </summary>
        /// <param name="refreshTime">the refresh time</param>
        /// <param name="updater">the item updater</param>
        /// <param name="log">the lykke log</param>
        public RefreshingDictionaryCache(TimeSpan refreshTime, IUpdater<T> updater, ILog log)
            : base(updater, refreshTime.Add(refreshTime))
        {
            _trigger = new TimerTrigger(nameof(AssetsService), refreshTime, log,
                async (x, y, token) => await Reset(token));
            _trigger.Start();
        }

        /// <inheritdoc />
        public void Dispose()
        {
            _trigger?.Dispose();
        }
    }
}
