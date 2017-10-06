using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IMarginAssetRepository
    {
        Task RegisterAssetAsync(IMarginAsset asset);

        Task EditAssetAsync(string id, IMarginAsset asset);

        Task<IEnumerable<IMarginAsset>> GetAssetsAsync();

        Task<IMarginAsset> GetAssetAsync(string id);
    }
}