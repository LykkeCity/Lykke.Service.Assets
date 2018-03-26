using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;

namespace Lykke.Service.Assets.Cache
{
    public class DistributedCache<I, T> where T : class, I
    {
        private readonly IDistributedCache _cache;
        private readonly string _partitionKey;
        private readonly DistributedCacheEntryOptions _cacheOptions;

        public DistributedCache(
            IDistributedCache cache,
            TimeSpan expiration,
            string partitionKey)
        {
            _cache = cache;
            _partitionKey = partitionKey;
            _cacheOptions = new DistributedCacheEntryOptions().SetAbsoluteExpiration(expiration);
        }

        private string GetCacheKey(string id)
        {
            return $":{_partitionKey}:{id}";
        }

        public async Task<T> GetAsync(string key, Func<Task<I>> factory)
        {
            var cached = await _cache.GetAsync(GetCacheKey(key));
            if (cached != null)
            {
                return CacheSerializer.Deserialize<T>(cached);
            }

            var result = await factory() as T;
            if (result != null)
            {
                await _cache.SetAsync(GetCacheKey(key), CacheSerializer.Serialize(result), _cacheOptions);
            }
            return result;
        }

        public async Task<IEnumerable<T>> GetListAsync(string key, Func<Task<IEnumerable<I>>> factory)
        {
            var cached = await _cache.GetAsync(GetCacheKey(key));
            if (cached != null)
            {
                return CacheSerializer.Deserialize<T[]>(cached);
            }

            var result = (await factory()).Cast<T>().ToArray();
            await _cache.SetAsync(GetCacheKey(key), CacheSerializer.Serialize(result), _cacheOptions);
            return result;
        }

        public async Task RemoveAsync(string id)
        {
            await _cache.RemoveAsync(GetCacheKey(id));
        }
    }
}
