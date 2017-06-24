using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetPairsManager
    {
        Task<IAssetPair> TryGetAsync(string assetPairId);
        Task<IEnumerable<IAssetPair>> GetAllAsync();
    }
}