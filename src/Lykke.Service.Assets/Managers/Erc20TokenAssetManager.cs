using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Managers
{
    public class Erc20TokenAssetManager : IErc20TokenAssetManager
    {
        private readonly ICache<IAsset>          _assetCache;
        private readonly IErc20TokenAssetService _tokenAssetService;


        public Erc20TokenAssetManager(
            IErc20TokenAssetService tokenAssetService,
            ICache<IAsset> assetCache)
        {
            _assetCache        = assetCache;
            _tokenAssetService = tokenAssetService;
        }

        public async Task<IAsset> CreateAssetForErc20TokenAsync(string tokenAddress)
        {
            await _assetCache.InvalidateAsync();

            return  await _tokenAssetService.CreateAssetForErc20TokenAsync(tokenAddress);
        }
    }
}
