using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Events;
using Lykke.Service.Assets.Client.ReadModels;

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
            var ids = _cache.Get<List<string>>(InMemoryAssetPairsReadModelRepository.AllKey);
            ids.Add(evt.Id);
            _cache.Set(InMemoryAssetPairsReadModelRepository.AllKey, ids);

            _cache.Set(evt.Id, Mapper.ToAssetPair(evt));
            return Task.CompletedTask;
        }

        public Task Handle(AssetPairUpdatedEvent evt)
        {
            _cache.Set(evt.Id, Mapper.ToAssetPair(evt));
            return Task.CompletedTask;
        }
    }
}
