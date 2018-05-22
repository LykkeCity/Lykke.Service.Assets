using Common;
using Common.Log;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Disposables;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Cache
{
    /// <inheritdoc />
    public class DictionaryCache<T> : IDictionaryCache<T>
        where T : ICacheItem
    {
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TimeSpan _cacheExpirationPeriod;

        private Dictionary<string, T> _items;
        private DateTime _cacheExpirationMoment;
        private bool _inAutoUpdate;

        /// <inheritdoc />
        public DictionaryCache(IDateTimeProvider dateTimeProvider, TimeSpan cacheExpirationPeriod)
        {
            _items = new Dictionary<string, T>();
            _cacheExpirationMoment = DateTime.MinValue;
            _dateTimeProvider = dateTimeProvider;
            _cacheExpirationPeriod = cacheExpirationPeriod;
        }

        /// <inheritdoc />
        public IDisposable StartAutoUpdate(string componentName, ILog log, Func<Task<IEnumerable<T>>> getAllAsync)
        {
            if (_inAutoUpdate)
            {
                throw new InvalidOperationException("Dictionary is already in auto update mode.");
            }

            _inAutoUpdate = true;
            async Task UpdateCache(ITimerTrigger trigger, TimerTriggeredHandlerArgs args, CancellationToken token)
            {
                await Update(getAllAsync);
            }

            var timer = new TimerTrigger(componentName, _cacheExpirationPeriod, log, UpdateCache);
            timer.Start();

            return Disposable.Create(() =>
            {
                _inAutoUpdate = false;
                timer.Dispose();
            });
        }

        /// <inheritdoc />
        public async Task EnsureCacheIsUpdatedAsync(Func<Task<IEnumerable<T>>> getAllItemsAsync)
        {
            if (_inAutoUpdate)
            {
                return;
            }

            if (_cacheExpirationMoment < _dateTimeProvider.UtcNow)
            {
                await Update(getAllItemsAsync);
            }
        }

        private async Task Update(Func<Task<IEnumerable<T>>> getAllItemsAsync)
        {
            var items = await getAllItemsAsync();
            Update(items);
        }

        /// <inheritdoc />
        public void Update(IEnumerable<T> items)
        {
            _items = items.ToDictionary(p => p.Id, p => p);

            _cacheExpirationMoment = _dateTimeProvider.UtcNow + _cacheExpirationPeriod;
        }

        /// <inheritdoc />
        public T TryGet(string id)
        {
            _items.TryGetValue(id, out var pair);

            return pair;
        }

        /// <inheritdoc />
        public IReadOnlyCollection<T> GetAll()
        {
            return _items.Values;
        }
    }
}
