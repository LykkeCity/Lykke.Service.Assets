using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetExtendedInfoService
    {
        Task<IAssetExtendedInfo> AddAsync(IAssetExtendedInfo assetInfo);

        IAssetExtendedInfo CreateDefault(string id);

        Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync();

        Task<IAssetExtendedInfo> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(IAssetExtendedInfo assetInfo);
    }
}