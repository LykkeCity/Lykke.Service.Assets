using Lykke.Service.Assets.Client.Events;
using Lykke.Service.Assets.Client.ReadModels;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Projections
{
    class AssetPairsProjection
    {
        private readonly IAssetPairsReadModelRepository _assetPairs;

        public AssetPairsProjection(IAssetPairsReadModelRepository assetPairs)
        {
            _assetPairs = assetPairs;
        }

        private Task Handle(AssetPairCreatedEvent evt)
        {
            _assetPairs.Add(Mapper.ToAssetPair(evt));
            return Task.CompletedTask;
        }

        public Task Handle(AssetPairUpdatedEvent evt)
        {
            _assetPairs.Update(Mapper.ToAssetPair(evt));
            return Task.CompletedTask;
        }
    }
}
