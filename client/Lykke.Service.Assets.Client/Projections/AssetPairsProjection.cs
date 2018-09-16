using Lykke.Service.Assets.Client.Events;
using Lykke.Service.Assets.Client.ReadModels;
using Microsoft.Extensions.Caching.Memory;
using System.Collections.Concurrent;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Projections
{
    class AssetPairsProjection
    {
        private readonly IMemoryCache _cache;

        public AssetPairsProjection(IMemoryCache cache)
        {
            _cache = cache;
        }

        private Task Handle(AssetPairCreatedEvent evt)
        {
            _cache.Set(evt.Id, Mapper.ToAssetPair(evt));

            var ids = _cache.Get<ConcurrentBag<string>>(InMemoryAssetPairsReadModelRepository.AllKey);
            ids.Add(evt.Id);
            _cache.Set(InMemoryAssetPairsReadModelRepository.AllKey, ids);

            return Task.CompletedTask;
        }

        public Task Handle(AssetPairUpdatedEvent evt)
        {
            _cache.Set(evt.Id, Mapper.ToAssetPair(evt));
            return Task.CompletedTask;
        }
    }
}
