using System;
using System.Threading;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Client.Cache
{
    public class RefreshingDictionaryCache<T> : DictionaryCache<T>, IDisposable
        where T : ICacheItem
    {
        private readonly TimerTrigger _trigger;

        public RefreshingDictionaryCache(TimeSpan expirationTime, IUpdater<T> updater, ILog log)
            : base(updater, expirationTime.Add(expirationTime))
        {
            _trigger = new TimerTrigger(nameof(AssetsService), expirationTime, log,
                async (x, y, token) => await UpdateCache(token));
            _trigger.Start();
        }

        private async Task UpdateCache(CancellationToken token)
        {
            var items = await RetrieveFromUpdater(token);
            InnerCache.Set(AllItems, items);
        }

        public void Dispose()
        {
            _trigger?.Dispose();
        }
    }
}
