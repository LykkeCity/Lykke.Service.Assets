using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetExtendedInfoRepository
    {
        Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync();

        Task<IAssetExtendedInfo> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpsertAsync(IAssetExtendedInfo assetInfo);
    }
}