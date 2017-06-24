using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetPairsRepository
    {
        Task<IEnumerable<IAssetPair>> GetAllAsync();
    }
}