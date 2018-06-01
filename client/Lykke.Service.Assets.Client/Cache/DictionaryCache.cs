using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Common.Cache;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Client.Cache
{
    /// <summary>
    /// Base class for a dictionary cache.
    /// </summary>
    internal class DictionaryCache<T> : IDictionaryCache<T>
        where T : ICacheItem
    {
        private const string AllItems = @"AllItems";
        private readonly OnDemandDataCache<Dictionary<string, T>> _innerCache;
        private readonly IUpdater<T> _updater;
        private readonly TimeSpan _expirationTime;

        /// <summary>
        /// Create new dictionary cache.
        /// </summary>
        protected DictionaryCache(IUpdater<T> updater, TimeSpan expirationTime)
        {
            _innerCache = new OnDemandDataCache<Dictionary<string, T>>();
            _updater = updater;
            _expirationTime = expirationTime;
        }

        /// <inheritdoc />
        public async Task Reset(CancellationToken token)
        {
            _innerCache.Remove(AllItems);
            await GetItems(token);
        }

        /// <inheritdoc />
        public async Task<T> TryGet(string id, CancellationToken token)
        {
            var items = await GetItems(token);
            items.TryGetValue(id, out var item);
            return item;
        }

        /// <inheritdoc />
        public async Task<IReadOnlyCollection<T>> GetAll(CancellationToken token)
        {
            var items = await GetItems(token);
            return items.Values;
        }

        private async Task<Dictionary<string, T>> GetItems(CancellationToken token)
        {
            async Task<Dictionary<string, T>> Refresh()
            {
                var items = await _updater.GetItemsAsync(token);
                return items.ToDictionary(x => x.Id);
            }

            return await _innerCache.GetOrAddAsync(AllItems, _ => Refresh(), _expirationTime);
        }
    }
}
