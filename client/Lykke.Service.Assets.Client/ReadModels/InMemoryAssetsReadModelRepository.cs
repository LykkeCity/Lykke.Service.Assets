using System.Collections.Concurrent;
using Autofac;
using Lykke.Service.Assets.Client.Models.v3;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.ReadModels
{
    class InMemoryAssetsReadModelRepository : IAssetsReadModelRepository, IStartable
    {
        public const string AllKey = "AllAssets";

        private readonly IAssetsService _assetsService;
        private readonly IMemoryCache _cache;

        public InMemoryAssetsReadModelRepository(IAssetsService assetsService, IMemoryCache cache)
        {
            _assetsService = assetsService;
            _cache = cache;
        }

        public Asset TryGet(string id)
        {
            try
            {
                _cache.TryGetValue(id, out Asset value);
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
            return ids.Select(x => _cache.Get<Asset>(x)).ToArray();
        }

        public void Start()
        {
            var assets = _assetsService.AssetGetAll(true);
            _cache.Set(AllKey, new ConcurrentBag<string>(assets.Select(x => x.Id)));
            foreach (var asset in assets)
            {
                _cache.Set(asset.Id, Mapper.ToAsset(asset));
            }
        }
    }
}
