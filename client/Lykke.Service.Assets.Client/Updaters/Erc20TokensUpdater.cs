using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client.Updaters
{
    internal class Erc20TokensUpdater : IUpdater<Erc20Token>
    {
        private readonly IAssetsService _service;

        public Erc20TokensUpdater(IAssetsService service)
        {
            _service = service;
        }

        public async Task<IEnumerable<Erc20Token>> GetItemsAsync(CancellationToken token)
        {
            var list = await _service.Erc20TokenGetAllWithAssetsAsync(token);

            return list?.Items;
        }
    }
}