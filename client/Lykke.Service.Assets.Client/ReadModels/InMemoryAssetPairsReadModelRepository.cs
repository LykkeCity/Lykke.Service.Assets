using Autofac;
using Lykke.Service.Assets.Client.Models.v3;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.ReadModels
{
    class InMemoryAssetPairsReadModelRepository : IAssetPairsReadModelRepository, IStartable
    {
        public const string AllKey = "AssetPairs";

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
                _cache.TryGetValue(GetKey(id), out AssetPair value);
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
            return ids.Select(x => _cache.Get<AssetPair>(GetKey(x))).ToArray();
        }

        public void Add(AssetPair assetPair)
        {
            _cache.Set(GetKey(assetPair), assetPair);

            var ids = _cache.Get<ConcurrentBag<string>>(AllKey);
            ids.Add(assetPair.Id);
            _cache.Set(AllKey, ids);
        }

        public void Update(AssetPair assetPair)
        {
            _cache.Set(GetKey(assetPair), assetPair);
        }

        public void Start()
        {
            var assetPairs = _assetsService.AssetPairGetAll();
            foreach (var assetPair in assetPairs)
            {
                _cache.Set(GetKey(assetPair.Id), Mapper.ToAssetPair(assetPair));
            }
            _cache.Set(AllKey, new ConcurrentBag<string>(assetPairs.Select(x => x.Id)));
        }

        private static string GetKey(AssetPair value)
        {
            return GetKey(value.Id);
        }

        private static string GetKey(string id)
        {
            return $"AssetPairs:{id}";
        }
    }
}
