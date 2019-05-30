using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client.Updaters
{
    internal class AssetPairsUpdater : IUpdater<AssetPair>
    {
        private readonly IAssetsService _service;

        public AssetPairsUpdater(IAssetsService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<AssetPair>> GetItemsAsync(CancellationToken token)
        {
            return await _service.AssetPairGetAllAsync(token);
        }
    }
}
