using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using JetBrains.Annotations;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    [UsedImplicitly]
    public class AssetPairsManager : IAssetPairsManager
    {
        private readonly IAssetPairsRepository _repository;
        private readonly IAssetPairsCacheService _cache;
        private readonly IDateTimeProvider _dateTimeProvider;
        private readonly TimeSpan _cacheExpirationPeriod;
        private DateTime _cacheExpirationMoment;

        public AssetPairsManager(IAssetPairsRepository repository, IAssetPairsCacheService cache, IDateTimeProvider dateTimeProvider, TimeSpan cacheExpirationPeriod)
        {
            _repository = repository;
            _cache = cache;
            _dateTimeProvider = dateTimeProvider;
            _cacheExpirationPeriod = cacheExpirationPeriod;

            _cacheExpirationMoment = DateTime.MinValue;
        }

        public async Task<IAssetPair> TryGetAsync(string assetPairId)
        {
            await EnsureCacheIsUpdatedAsync();

            return _cache.TryGetPair(assetPairId);
        }

        public async Task<IEnumerable<IAssetPair>> GetAllAsync()
        {
            await EnsureCacheIsUpdatedAsync();

            return _cache.GetAll();
        }

        private async Task EnsureCacheIsUpdatedAsync()
        {
            if (_cacheExpirationMoment < _dateTimeProvider.UtcNow)
            {
                await UpdateCacheAsync();
                _cacheExpirationMoment = _dateTimeProvider.UtcNow + _cacheExpirationPeriod;
            }
        }

        private async Task UpdateCacheAsync()
        {
            var pairs = await _repository.GetAllAsync();

            _cache.Update(pairs);
        }
    }
}