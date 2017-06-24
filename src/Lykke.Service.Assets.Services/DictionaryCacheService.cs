using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    [UsedImplicitly]
    public class DictionaryCacheService<TDictionaryItem> : IDictionaryCacheService<TDictionaryItem> 
        where TDictionaryItem : IDictionaryItem
    {
        private Dictionary<string, TDictionaryItem> _items = new Dictionary<string, TDictionaryItem>();

        public void Update(IEnumerable<TDictionaryItem> items)
        {
            _items = items.ToDictionary(p => p.Id, p => p);
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