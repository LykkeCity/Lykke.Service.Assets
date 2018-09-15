using Lykke.Service.Assets.Client.Events;
using Lykke.Service.Assets.Client.ReadModels;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Projections
{
    class AssetsProjection
    {
        private readonly IMemoryCache _cache;

        public AssetsProjection(IMemoryCache cache)
        {
            _cache = cache;
        }

        private Task Handle(AssetCreatedEvent evt)
        {
            _cache.Set(evt.Id, Mapper.ToAsset(evt));

            var ids = _cache.Get<ConcurrentBag<string>>(InMemoryAssetsReadModelRepository.AllKey);
            ids.Add(evt.Id);
            _cache.Set(InMemoryAssetsReadModelRepository.AllKey, ids);

            return Task.CompletedTask;
        }

        public Task Handle(AssetUpdatedEvent evt)
        {
            _cache.Set(evt.Id, Mapper.ToAsset(evt));
            return Task.CompletedTask;
        }
    }
}
