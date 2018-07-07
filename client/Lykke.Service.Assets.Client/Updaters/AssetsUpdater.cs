using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client.Updaters
{
    internal class AssetsUpdater : IUpdater<Asset>
    {
        private readonly IAssetsService _service;

        public AssetsUpdater(IAssetsService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<Asset>> GetItemsAsync(CancellationToken token)
        {
            return await _service.AssetGetAllAsync(true, token);
        }
    }
}
