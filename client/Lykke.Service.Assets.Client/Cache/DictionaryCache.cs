using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Common.Cache;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Client.Cache
{
    public class DictionaryCache<T> : IDictionaryCache<T>
        where T : ICacheItem
    {
        protected const string AllItems = @"AllItems";

        private readonly IUpdater<T> _updater;
        private readonly TimeSpan _expirationTime;

        protected DictionaryCache(IUpdater<T> updater, TimeSpan expirationTime)
        {
            InnerCache = new OnDemandDataCache<Dictionary<string, T>>();
            _updater = updater;
            _expirationTime = expirationTime;
        }

        protected OnDemandDataCache<Dictionary<string, T>> InnerCache { get; }

        public async Task Reset(CancellationToken token)
        {
            InnerCache.Remove(AllItems);
            await GetItems(token);
        }

        public async Task<T> TryGet(string id, CancellationToken token)
        {
            var items = await GetItems(token);
            items.TryGetValue(id, out var item);
            return item;
        }

        public async Task<IReadOnlyCollection<T>> GetAll(CancellationToken token)
        {
            var items = await GetItems(token);
            return items.Values;
        }

        private async Task<Dictionary<string, T>> GetItems(CancellationToken token)
        {
            return await InnerCache.GetOrAddAsync(AllItems,
                async _ => await RetrieveFromUpdater(token)
                , _expirationTime);
        }

        protected async Task<Dictionary<string, T>> RetrieveFromUpdater(CancellationToken token)
        {
            var items = await _updater.GetItemsAsync(token);
            return items.ToDictionary(x => x.Id);
        }
    }
}
