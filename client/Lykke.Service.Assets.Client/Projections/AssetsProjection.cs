using Microsoft.Extensions.Caching.Memory;
using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Events;
using Lykke.Service.Assets.Client.ReadModels;

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
            var ids = _cache.Get<List<string>>(AssetsReadModel.AllKey);
            ids.Add(evt.Id);
            _cache.Set(AssetsReadModel.AllKey, ids);

            _cache.Set(evt.Id, Mapper.ToAsset(evt));
            return Task.CompletedTask;
        }

        public Task Handle(AssetUpdatedEvent evt)
        {
            _cache.Set(evt.Id, Mapper.ToAsset(evt));
            return Task.CompletedTask;
        }
    }
}
