using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IDictionaryManager<TDictionaryItem>
        where TDictionaryItem : IDictionaryItem
    {
        Task<TDictionaryItem> TryGetAsync(string id);
        Task<IEnumerable<TDictionaryItem>> GetAllAsync();
    }
}