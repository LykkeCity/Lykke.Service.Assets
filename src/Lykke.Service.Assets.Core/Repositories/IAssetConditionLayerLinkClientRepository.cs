using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IAssetConditionLayerLinkClientRepository
    {
        Task<IEnumerable<string>> GetLayersAsync(string clientId);

        Task AddAsync(string clientId, string layerId);

        Task RemoveAsync(string clientId, string layerId);

        /// <summary>
        /// Remove link to LayerId from all clients
        /// </summary>
        /// <param name="layerId"></param>
        /// <returns></returns>
        Task RemoveLayerFromClientsAsync(string layerId);
    }
}
