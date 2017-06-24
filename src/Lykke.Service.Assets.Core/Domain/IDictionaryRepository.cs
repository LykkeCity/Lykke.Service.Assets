using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IDictionaryRepository<TDictionaryItem>
        where TDictionaryItem : IDictionaryItem
    {
        Task<IEnumerable<TDictionaryItem>> GetAllAsync();
    }
}