using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Custom
{
    public interface IDictionaryCache<TDictionaryItem>
        where TDictionaryItem : IDictionaryItemModel
    {
        Task EnsureCacheIsUpdatedAsync(Func<Task<IEnumerable<TDictionaryItem>>> getAllItemsAsync);
        TDictionaryItem TryGet(string id);
        IReadOnlyCollection<TDictionaryItem> GetAll();
    }
}