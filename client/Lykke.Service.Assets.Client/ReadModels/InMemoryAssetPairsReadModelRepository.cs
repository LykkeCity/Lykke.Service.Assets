using System.Collections.Concurrent;
using Autofac;
using Lykke.Service.Assets.Client.Models.v3;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.ReadModels
{
    class InMemoryAssetPairsReadModelRepository : IAssetPairsReadModelRepository, IStartable
    {
        public const string AllKey = "AllAssetPairs";

        private readonly IAssetsService _assetsService;
        private readonly IMemoryCache _cache;

        public InMemoryAssetPairsReadModelRepository(IAssetsService assetsService, IMemoryCache cache)
        {
            _assetsService = assetsService;
            _cache = cache;
        }

        public AssetPair TryGet(string id)
        {
            try
            {
                _cache.TryGetValue(id, out AssetPair value);
                return value;
            }
            catch (System.InvalidCastException)
            {
                return null;
            }
        }

        public IReadOnlyCollection<AssetPair> GetAll()
        {
            var ids = _cache.Get<ConcurrentBag<string>>(AllKey);
            return ids.Select(x => _cache.Get<AssetPair>(x)).ToArray();
        }

        public void Start()
        {
            var assetPairs = _assetsService.AssetPairGetAll();
            _cache.Set(AllKey, new ConcurrentBag<string>(assetPairs.Select(x => x.Id)));
            foreach (var assetPair in assetPairs)
            {
                _cache.Set(assetPair.Id, Mapper.ToAssetPair(assetPair));
            }
        }
    }
}
