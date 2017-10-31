using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Memory;

namespace Lykke.Service.Assets.Cache
{
    public class Cache<T> : ICache<T>
    {
        private const string KeyPrefix = "Lykke.Service.Assets";
        

        public Cache(
            IMemoryCache innerCache,
            string partitionKey,
            TimeSpan cacheDuration)
        {
            CacheDuration = cacheDuration;
            InnerCache    = innerCache;
            PartitionKey  = partitionKey;
        }


        protected TimeSpan CacheDuration { get; }

        protected IMemoryCache InnerCache { get; }

        protected string PartitionKey { get; }


        public virtual async Task<T> GetAsync(string itemKey, Func<Task<T>> factory)
        {
            return await GetAsync(PartitionKey, itemKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;

                return factory();
            });
        }
        
        protected async Task<T> GetAsync(string partitionKey, string itemKey, Func<ICacheEntry, Task<T>> factory)
        {
            var recordKey = $"{KeyPrefix}:{partitionKey}:{itemKey}";
            var result    = await InnerCache.GetOrCreateAsync(recordKey, factory);

            UpdateIndex(partitionKey, recordKey);

            return result;
        }

        public virtual async Task<IEnumerable<T>> GetListAsync(string listKey, Func<Task<IEnumerable<T>>> factory)
        {
            return await GetListAsync(PartitionKey, listKey, entry =>
            {
                entry.AbsoluteExpirationRelativeToNow = CacheDuration;

                return factory();
            });
        }

        protected async Task<IEnumerable<T>> GetListAsync(string partitionKey, string listKey, Func<ICacheEntry, Task<IEnumerable<T>>> factory)
        {
            var recordKey = $"{KeyPrefix}:ListOf{partitionKey}:{listKey}";
            var result    = await InnerCache.GetOrCreateAsync(recordKey, factory);

            UpdateIndex(partitionKey, recordKey);

            return result;
        }

        public virtual async Task InvalidateAsync()
        {
            await InvalidateAsync(PartitionKey);
        }

        protected async Task InvalidateAsync(string partitionKey)
        {
            await Task.Run(() =>
            {
                if (InnerCache.TryGetValue(partitionKey, out HashSet<string> index))
                {
                    Parallel.ForEach(index, InnerCache.Remove);

                    InnerCache.Remove(partitionKey);
                }
            });
        }
        
        private void UpdateIndex(string partionKey, string key)
        {
            var index = InnerCache.GetOrCreate(partionKey, entry => new HashSet<string>());

            index.Add(key);

            InnerCache.Set(partionKey, index, CacheDuration);
        }
    }
}
