using System;
using System.Runtime.CompilerServices;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
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
        [Obsolete("Please, use the overload with ILogFactory parameter.")]
        public RefreshingDictionaryCache(TimeSpan refreshTime, IUpdater<T> updater, ILog log)
            : base(updater, refreshTime.Add(refreshTime))
        {
            _trigger = new TimerTrigger(nameof(AssetsService), refreshTime, log,
                async (x, y, token) => await Reset(token));
            _trigger.Start();
        }

        /// <summary>
        /// Creates a new refreshing dictionary cache.
        /// </summary>
        /// <param name="refreshTime">The refresh time.</param>
        /// <param name="updater">The item updater instance.</param>
        /// <param name="logFactory">The lykke log factory instance.</param>
        [UsedImplicitly]
        public RefreshingDictionaryCache(TimeSpan refreshTime, IUpdater<T> updater, ILogFactory logFactory)
            : base(updater, refreshTime.Add((refreshTime)))
        {
            _trigger = new TimerTrigger(nameof(AssetsService), refreshTime, logFactory,
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
