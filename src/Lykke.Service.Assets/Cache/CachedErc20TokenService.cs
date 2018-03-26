using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.Service.Assets.Tools;

namespace Lykke.Service.Assets.Cache
{
    public class CachedErc20TokenService : ICachedErc20TokenService
    {
        private readonly IErc20TokenService _tokenService;
        private readonly DistributedCache<IErc20Token, Erc20TokenEntity> _cache;
        private const string AllEntitiesKey = "All";
        private const string WithAssetsKey = "WithAssets";

        public CachedErc20TokenService(
            IErc20TokenService tokenService,
            DistributedCache<IErc20Token, Erc20TokenEntity> cache)
        {
            _cache = cache;
            _tokenService = tokenService;
        }

        public async Task<IErc20Token> AddAsync(IErc20Token token)
        {
            await InvalidateCache();

            await _tokenService.AddAsync(token);

            return token;
        }

        public async Task<IEnumerable<IErc20Token>> GetAllAsync()
        {
            return await _cache.GetListAsync(AllEntitiesKey, _tokenService.GetAllAsync);
        }

        public async Task<IEnumerable<IErc20Token>> GetByAssetIdsAsync(string[] assetIds)
        {
            var tokens = await GetAllWithAssetsAsync();
            return tokens.Where(x => assetIds.Contains(x.AssetId));
        }

        public async Task<IErc20Token> GetByAssetIdAsync(string assetId)
        {
            return await _cache.GetAsync(assetId, () => _tokenService.GetByAssetIdAsync(assetId));
        }

        public async Task UpdateAsync(IErc20Token token)
        {
            await InvalidateCache();
            await _tokenService.UpdateAsync(token);
        }

        public async Task<IErc20Token> GetByTokenAddressAsync(string tokenAddress)
        {
            return await _tokenService.GetByTokenAddressAsync(tokenAddress);
        }

        public async Task<IEnumerable<IErc20Token>> GetAllWithAssetsAsync()
        {
            return await _cache.GetListAsync(WithAssetsKey, _tokenService.GetAllWithAssetsAsync);
        }

        private async Task InvalidateCache(string id = null)
        {
            if (id != null)
            {
                await _cache.RemoveAsync(id);
            }
            await _cache.RemoveAsync(AllEntitiesKey);
            await _cache.RemoveAsync(WithAssetsKey);
        }
    }
}
