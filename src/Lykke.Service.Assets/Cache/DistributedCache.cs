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

        public async Task RemoveAsync(string id)
        {
            await _redisDatabase.KeyDeleteAsync(GetCacheKey(id));
        }
    }
}
