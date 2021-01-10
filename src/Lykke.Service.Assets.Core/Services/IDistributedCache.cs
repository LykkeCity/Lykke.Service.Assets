using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IDistributedCache<I, T> where T : class, I
    {
        string GetCacheKey(string id);
        Task<T> GetAsync(string key, Func<Task<I>> factory);
        Task<IEnumerable<T>> GetListAsync(string key, Func<Task<IEnumerable<I>>> factory);

        Task<IEnumerable<T>> GetListAsync(
            string prefix,
            ICollection<string> keys,
            Func<T, string> keyExtractor,
            Func<IEnumerable<string>, Task<IEnumerable<I>>> factory);

        Task RemoveAsync(string id);

        Task CacheDataAsync<T>(
            IEnumerable<T> items,
            Func<T, string> keyExtractor,
            string prefix);

        Task CacheDataAsync<T>(string key, IEnumerable<T> items);
    }
}
