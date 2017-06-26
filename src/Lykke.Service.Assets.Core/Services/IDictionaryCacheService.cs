using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IDictionaryCacheService<TDictionaryItem>
        where TDictionaryItem : IDictionaryItem
    {
        void Update(IEnumerable<TDictionaryItem> item);
        TDictionaryItem TryGet(string id);
        IReadOnlyCollection<TDictionaryItem> GetAll();
    }
}