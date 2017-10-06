using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetDescriptionRepository
    {
        Task SaveAsync(IAssetDescription src);

        Task<IAssetDescription> GetAssetExtendedInfoAsync(string id);

        Task<IEnumerable<IAssetDescription>> GetAllAsync();
    }
}