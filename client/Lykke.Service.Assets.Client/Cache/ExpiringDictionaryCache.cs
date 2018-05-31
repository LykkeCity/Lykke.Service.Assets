using System;
using Lykke.Service.Assets.Client.Updaters;

namespace Lykke.Service.Assets.Client.Cache
{
    public class ExpiringDictionaryCache<T> : DictionaryCache<T>
        where T : ICacheItem
    {
        public ExpiringDictionaryCache(TimeSpan expirationTime, IUpdater<T> updater)
            : base(updater, expirationTime)
        {
        }
    }
}
