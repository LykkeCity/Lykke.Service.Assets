using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Cache
{
    public class DictionaryCache<T> : IDictionaryCache<T>
        where T : ICacheItem
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TimeSpan          _cacheExpirationPeriod;

        private Dictionary<string, T> _items;
        private DateTime              _cacheExpirationMoment;


        public DictionaryCache(IDateTimeProvider dateTimeProvider, TimeSpan cacheExpirationPeriod)
        {
            _items                 = new Dictionary<string, T>();
            _cacheExpirationMoment = DateTime.MinValue;
            _dateTimeProvider      = dateTimeProvider;
            _cacheExpirationPeriod = cacheExpirationPeriod;
        }

        public async Task EnsureCacheIsUpdatedAsync(Func<Task<IEnumerable<T>>> getAllItemsAsync)
        {
            if (_cacheExpirationMoment < _dateTimeProvider.UtcNow)
            {
                var items = await getAllItemsAsync();

                Update(items);
            }
        }

        public void Update(IEnumerable<T> items)
        {
            _items = items.ToDictionary(p => p.Id, p => p);

            _cacheExpirationMoment = _dateTimeProvider.UtcNow + _cacheExpirationPeriod;
        }

        public T TryGet(string id)
        {
            _items.TryGetValue(id, out var pair);

            return pair;
        }

        public IReadOnlyCollection<T> GetAll()
        {
            return _items.Values;
        }
    }
}
