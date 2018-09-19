using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Responses.V2;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Cache
{
    public class CachedErc20TokenService : ICachedErc20TokenService
    {
        private readonly IErc20TokenService _tokenService;
        private readonly DistributedCache<IErc20Token, Erc20Token> _cache;
        private const string WithAssetsKey = "WithAssets";

        public CachedErc20TokenService(
            IErc20TokenService tokenService,
            DistributedCache<IErc20Token, Erc20Token> cache)
        {
            _cache = cache;
            _tokenService = tokenService;
        }

        public async Task<Erc20Token> AddAsync(IErc20Token token)
        {
            await InvalidateCache();

            await _tokenService.AddAsync(token);

            return AutoMapper.Mapper.Map<Erc20Token>(token);
        }

        public async Task<IEnumerable<Erc20Token>> GetByAssetIdsAsync(string[] assetIds)
        {
            if (assetIds == null || assetIds.Length == 0)
                return new Erc20Token[0];

            if (assetIds.Length == 1)
            {
                var token = await GetByAssetIdAsync(assetIds[0]);
                if (token == null)
                {
                    return new Erc20Token[0];
                }
                else
                {
                    return new[] { token };
                }
            }

            var tokens = await GetAllWithAssetsAsync();
            return tokens.Where(x => assetIds.Contains(x.AssetId));
        }

        public Task<Erc20Token> GetByAssetIdAsync(string assetId)
        {
            return _cache.GetAsync(assetId,
                async () => AutoMapper.Mapper.Map<Erc20Token>(await _tokenService.GetByAssetIdAsync(assetId)));
        }

        public async Task UpdateAsync(IErc20Token token)
        {
            await InvalidateCache();
            await _tokenService.UpdateAsync(token);
        }

        public Task<Erc20Token> GetByTokenAddressAsync(string tokenAddress)
        {
            return _cache.GetAsync(tokenAddress,
                async () => AutoMapper.Mapper.Map<Erc20Token>(await _tokenService.GetByTokenAddressAsync(tokenAddress)));
        }

        public async Task<IEnumerable<Erc20Token>> GetAllWithAssetsAsync()
        {
            return await _cache.GetListAsync(WithAssetsKey,
                async () => AutoMapper.Mapper.Map<List<Erc20Token>>(await _tokenService.GetAllWithAssetsAsync()));
        }

        private async Task InvalidateCache(string id = null)
        {
            if (id != null)
            {
                await _cache.RemoveAsync(id);
            }
            await _cache.RemoveAsync(WithAssetsKey);
        }
    }
}
