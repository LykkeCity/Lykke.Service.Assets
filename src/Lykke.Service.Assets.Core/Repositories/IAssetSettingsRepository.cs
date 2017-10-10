using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetSettingsRepository
    {
        Task<IEnumerable<IAssetSettings>> GetAllAsync();

        Task<IAssetSettings> GetAsync(string asset);

        Task RemoveAsync(string asset);

        Task UpsertAsync(IAssetSettings settings);
    }
}