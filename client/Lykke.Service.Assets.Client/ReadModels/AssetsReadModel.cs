using Autofac;
using Lykke.Service.Assets.Client.Models.v3;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.ReadModels
{
    class AssetsReadModel : IAssetsReadModel, IStartable
    {
        public const string AllKey = "AllAssets";

        private readonly IAssetsService _assetsService;
        private readonly IMemoryCache _cache;

        public AssetsReadModel(IAssetsService assetsService, IMemoryCache cache)
        {
            _assetsService = assetsService;
            _cache = cache;
        }

        public Asset Get(string id)
        {
            try
            {
                if (!_cache.TryGetValue(id, out Asset value))
                    return null;
                return value;
            }
            catch (System.InvalidCastException)
            {
                return null;
            }
        }

        public IReadOnlyCollection<Asset> GetAll()
        {
            var ids = _cache.Get<List<string>>(AllKey);
            return ids.Select(x => _cache.Get<Asset>(x)).ToArray();
        }

        public void Start()
        {
            var assets = _assetsService.AssetGetAll(true);
            foreach (var asset in assets)
            {
                _cache.Set(asset.Id, Mapper.ToAsset(asset));
            }
        }
    }
}
