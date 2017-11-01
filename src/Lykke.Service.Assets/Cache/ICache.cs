using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Cache
{
    public interface ICache<T>
    {
        Task<T> GetAsync(string itemKey, Func<Task<T>> factory);

        Task<IEnumerable<T>> GetListAsync(string listKey, Func<Task<IEnumerable<T>>> factory);

        Task InvalidateAsync();
    }
}
