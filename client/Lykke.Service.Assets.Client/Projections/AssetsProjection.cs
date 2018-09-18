using Lykke.Service.Assets.Client.Events;
using Lykke.Service.Assets.Client.ReadModels;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Projections
{
    class AssetsProjection
    {
        private readonly IAssetsReadModelRepository _assets;

        public AssetsProjection(IAssetsReadModelRepository assets)
        {
            _assets = assets;
        }

        private Task Handle(AssetCreatedEvent evt)
        {
            _assets.Add(Mapper.ToAsset(evt));
            return Task.CompletedTask;
        }

        public Task Handle(AssetUpdatedEvent evt)
        {
            _assets.Update(Mapper.ToAsset(evt));
            return Task.CompletedTask;
        }
    }
}
