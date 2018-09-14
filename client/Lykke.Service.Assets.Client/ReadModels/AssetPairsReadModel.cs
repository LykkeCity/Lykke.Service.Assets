using Autofac;
using Lykke.Service.Assets.Client.Models.v3;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.ReadModels
{
    class AssetPairsReadModel : IAssetPairsReadModel, IStartable
    {
        public const string AllKey = "AllAssetPairs";

        private readonly IAssetsService _assetsService;
        private readonly IMemoryCache _cache;

        public AssetPairsReadModel(IAssetsService assetsService, IMemoryCache cache)
        {
            _assetsService = assetsService;
            _cache = cache;
        }

        public AssetPair Get(string id)
        {
            try
            {
                if (!_cache.TryGetValue(id, out AssetPair value))
                    return null;
                return value;
            }
            catch (System.InvalidCastException)
            {
                return null;
            }
        }

        public IReadOnlyCollection<AssetPair> GetAll()
        {
            var ids = _cache.Get<List<string>>(AllKey);
            return ids.Select(x => _cache.Get<AssetPair>(x)).ToArray();
        }

        public void Start()
        {
            var assetPairs = _assetsService.AssetPairGetAll();
            _cache.Set(AllKey, assetPairs.Select(x => x.Id).ToList());
            foreach (var assetPair in assetPairs)
            {
                _cache.Set(assetPair.Id, Mapper.ToAssetPair(assetPair));
            }
        }
    }
}
