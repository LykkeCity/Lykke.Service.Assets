using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace Lykke.Service.Assets.Cache
{
    public class DistributedCache<I, T> where T : class, I
    {
        private readonly IDatabase _redisDatabase;
        private readonly string _partitionKey;
        private readonly TimeSpan _expiration;

        public DistributedCache(
            IDatabase redisDatabase,
            TimeSpan expiration,
            string partitionKey)
        {
            _redisDatabase = redisDatabase;
            _partitionKey = partitionKey;
            _expiration = expiration;
        }

        private string GetCacheKey(string id)
        {
            return $"{_partitionKey}:{id}";
        }

        public async Task<T> GetAsync(string key, Func<Task<I>> factory)
        {
            var cached = await _redisDatabase.StringGetAsync(GetCacheKey(key));
            if (cached.HasValue)
            {
                return CacheSerializer.Deserialize<T>(cached);
            }

            var result = await factory() as T;
            if (result != null)
            {
                await _redisDatabase.StringSetAsync(GetCacheKey(key), CacheSerializer.Serialize(result), _expiration);
            }
            return result;
        }

        public async Task<IEnumerable<T>> GetListAsync(string key, Func<Task<IEnumerable<I>>> factory)
        {
            var cached = await _redisDatabase.StringGetAsync(GetCacheKey(key));
            if (cached.HasValue)
            {
                return CacheSerializer.Deserialize<T[]>(cached);
            }

            var result = (await factory()).Cast<T>().ToArray();
            await _redisDatabase.StringSetAsync(GetCacheKey(key), CacheSerializer.Serialize(result), _expiration);
            return result;
        }

        public async Task<IEnumerable<T>> GetListAsync(
            string prefix,
            ICollection<string> keys,
            Func<T, string> keyExtractor,
            Func<IEnumerable<string>, Task<IEnumerable<I>>> factory)
        {
            IEnumerable<T> cachedItems;
            List<string> notFoundKeys;
            if (keys == null)
            {
                var cached = await _redisDatabase.StringGetAsync(GetCacheKey(prefix));
                if (cached.HasValue)
                {
                    var resultItems = CacheSerializer.Deserialize<T[]>(cached);
                    foreach (var resultItem in resultItems)
                    {
                        var key = keyExtractor(resultItem);
                        await _redisDatabase.StringSetAsync(GetCacheKey($"{prefix}:{key}"), CacheSerializer.Serialize(resultItem), _expiration);
                    }
                    return resultItems;
                }
                cachedItems = new T[0];
                notFoundKeys = null;
            }
            else
            {
                var cached = await _redisDatabase.StringGetAsync(keys.Select(k => (RedisKey)GetCacheKey($"{prefix}:{k}")).ToArray());
                cachedItems = cached.Select(c => CacheSerializer.Deserialize<T>(c));
                if (cached.Length == keys.Count)
                    return cachedItems;
                notFoundKeys = new List<string>();
                var cachedKeysHash = new HashSet<string>(cachedItems.Select(keyExtractor));
                foreach (var key in keys)
                {
                    if (cachedKeysHash.Contains(key))
                        continue;
                    notFoundKeys.Add(key);
                }
            }

            var foundResults = (await factory(notFoundKeys)).Cast<T>();
            foreach (var foundResult in foundResults)
            {
                var key = keyExtractor(foundResult);
                await _redisDatabase.StringSetAsync(GetCacheKey($"{prefix}:{key}"), CacheSerializer.Serialize(foundResult), _expiration);
            }

            return cachedItems.Concat(foundResults);
        }

        public async Task RemoveAsync(string id)
        {
            await _redisDatabase.KeyDeleteAsync(GetCacheKey(id));
        }
    }
}
