using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IErc20TokenAssetService
    {
        Task<IAsset> CreateAssetForErc20TokenAsync(string tokenAddress);
    }
}