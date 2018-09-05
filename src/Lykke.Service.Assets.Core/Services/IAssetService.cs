using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;


namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetService
    {
        Task<IAsset> AddAsync(IAsset asset);

        IAsset CreateDefault();

        Task DisableAsync(string id);

        Task EnableAsync(string id);

        Task<IEnumerable<IAsset>> GetAllAsync(bool includeNonTradable);

        Task<IEnumerable<IAsset>> GetAsync(string[] ids, bool? isTradable);

        Task<IAsset> GetAsync(string id);
        
        Task RemoveAsync(string id);

        Task UpdateAsync(IAsset asset);
    }
}
