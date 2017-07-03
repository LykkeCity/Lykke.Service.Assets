using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Custom
{
    public class DictionaryCache<TDictionaryItem> : IDictionaryCache<TDictionaryItem> 
        where TDictionaryItem : IDictionaryItemModel
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TimeSpan _cacheExpirationPeriod;

        private Dictionary<string, TDictionaryItem> _items = new Dictionary<string, TDictionaryItem>();
        private DateTime _cacheExpirationMoment;

        public DictionaryCache(IDateTimeProvider dateTimeProvider, TimeSpan cacheExpirationPeriod)
        {
            _dateTimeProvider = dateTimeProvider;
            _cacheExpirationPeriod = cacheExpirationPeriod;

            _cacheExpirationMoment = DateTime.MinValue;
        }

        public async Task EnsureCacheIsUpdatedAsync(Func<Task<IEnumerable<TDictionaryItem>>> getAllItemsAsync)
        {
            if (_cacheExpirationMoment < _dateTimeProvider.UtcNow)
            {
                var items = await getAllItemsAsync();

                Update(items);
            }
        }

        public void Update(IEnumerable<TDictionaryItem> items)
        {
            _items = items.ToDictionary(p => p.Id, p => p);

            _cacheExpirationMoment = _dateTimeProvider.UtcNow + _cacheExpirationPeriod;
        }

        public TDictionaryItem TryGet(string id)
        {
            _items.TryGetValue(id, out TDictionaryItem pair);

            return pair;
        }

        public IReadOnlyCollection<TDictionaryItem> GetAll()
        {
            return _items.Values;
        }
    }
}