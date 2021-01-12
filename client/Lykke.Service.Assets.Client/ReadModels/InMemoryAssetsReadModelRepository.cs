using Autofac;
using Lykke.Service.Assets.Client.Models.v3;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.ReadModels
{
    class InMemoryAssetsReadModelRepository : IAssetsReadModelRepository, IStartable
    {
        private const string AllKey = "Assets";

        private readonly IAssetsServiceHttp _assetsService;
        private readonly IMemoryCache _cache;

        public InMemoryAssetsReadModelRepository(IAssetsServiceHttp assetsService, IMemoryCache cache)
        {
            _assetsService = assetsService;
            _cache = cache;
        }

        public Asset TryGet(string id)
        {
            try
            {
                _cache.TryGetValue(GetKey(id), out Asset value);
                return value;
            }
            catch (System.InvalidCastException)
            {
                return null;
            }
        }

        public IReadOnlyCollection<Asset> GetAll()
        {
            var ids = _cache.Get<ConcurrentBag<string>>(AllKey);
            return ids.Select(x => _cache.Get<Asset>(GetKey(x))).ToArray();
        }

        public void Add(Asset assetPair)
        {
            _cache.Set(GetKey(assetPair), assetPair);

            var ids = _cache.Get<ConcurrentBag<string>>(AllKey);
            ids.Add(assetPair.Id);
            _cache.Set(AllKey, ids);
        }

        public void Update(Asset assetPair)
        {
            _cache.Set(GetKey(assetPair), assetPair);
        }

        public void Start()
        {
            var assets = _assetsService.AssetGetAll(true);
            foreach (var asset in assets)
            {
                _cache.Set(GetKey(asset.Id), Mapper.ToAsset(asset));
            }
            _cache.Set(AllKey, new ConcurrentBag<string>(assets.Select(x => x.Id)));
        }

        private static string GetKey(Asset value)
        {
            return GetKey(value.Id);
        }

        private static string GetKey(string id)
        {
            return $"Assets:{id}";
        }
    }
}
