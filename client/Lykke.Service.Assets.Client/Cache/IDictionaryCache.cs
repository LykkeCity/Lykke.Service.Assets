using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Cache
{
    public interface IDictionaryCache<T>
        where T : ICacheItem
    {
        Task EnsureCacheIsUpdatedAsync(Func<Task<IEnumerable<T>>> getAllAsync);

        void Update(IEnumerable<T> items);

        T TryGet(string id);

        IReadOnlyCollection<T> GetAll();
    }
}
