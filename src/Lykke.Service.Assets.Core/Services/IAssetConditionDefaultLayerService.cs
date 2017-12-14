using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetConditionDefaultLayerService
    {
        Task<IAssetConditionDefaultLayer> GetAsync();

        Task InsertOrUpdateAsync(IAssetConditionDefaultLayer settings);
    }
}
