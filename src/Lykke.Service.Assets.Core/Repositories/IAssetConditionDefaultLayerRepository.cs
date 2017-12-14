using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionDefaultLayerRepository
    {
        Task<IAssetConditionDefaultLayer> GetAsync();
        Task InsertOrUpdate(IAssetConditionDefaultLayer settings);
    }
}
