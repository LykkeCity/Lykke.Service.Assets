using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using System.Linq;
using Lykke.Service.Assets.Tools;

namespace Lykke.Service.Assets.Managers
{
    public class Erc20TokenManager : IErc20TokenManager
    {
        private readonly ICache<IErc20Token> _tokenCache;
        private readonly IErc20TokenService _tokenService;

        public Erc20TokenManager(
            IErc20TokenService tokenService,
            ICache<IErc20Token> tokenCache)
        {
            _tokenCache = tokenCache;
            _tokenService = tokenService;
        }

        public async Task<IErc20Token> AddAsync(IErc20Token token)
        {
            await _tokenService.AddAsync(token);
            await InvalidateCache();

            return token;
        }

        public async Task<IEnumerable<IErc20Token>> GetAllAsync()
        {
            return await _tokenCache.GetListAsync("All", _tokenService.GetAllAsync);
        }

        public async Task<IEnumerable<IErc20Token>> GetByAssetIdsAsync(string[] assetIds)
        {
            var listKey = assetIds.GetMD5();

            return await _tokenCache.GetListAsync($"ForIdsList:{listKey}", () => _tokenService.GetByAssetIdsAsync(assetIds));
        }

        public async Task<IErc20Token> GetByAssetIdAsync(string assetId)
        {
            return await _tokenCache.GetAsync(assetId, () => _tokenService.GetByAssetIdAsync(assetId));
        }

        public async Task UpdateAsync(IErc20Token token)
        {
            await _tokenService.UpdateAsync(token);
            await InvalidateCache();
        }

        public async Task<IErc20Token> GetByTokenAddressAsync(string tokenAddress)
        {
            return await _tokenService.GetByTokenAddressAsync(tokenAddress);
        }

        public async Task<IEnumerable<IErc20Token>> GetAllWithAssetsAsync()
        {
            return await _tokenCache.GetListAsync("WithAssets", _tokenService.GetAllWithAssetsAsync);
        }

        public async Task InvalidateCache()
        {
            await _tokenCache.InvalidateAsync();
        }
    }
}
