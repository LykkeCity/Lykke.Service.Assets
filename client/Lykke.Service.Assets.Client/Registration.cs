using Lykke.Cqrs.Configuration;
using Lykke.Cqrs.Configuration.BoundedContext;
using Lykke.Service.Assets.Client.Events;
using Lykke.Service.Assets.Client.Projections;

namespace Lykke.Service.Assets.Client
{
    /// <summary>
    /// Extensions for the IBoundedContextRegistration.
    /// </summary>
    public static class Registration
    {
        /// <summary>
        /// Use the default in-memory assets and asset-pairs read model.
        /// </summary>
        public static IBoundedContextRegistration WithAssetsReadModel(this IBoundedContextRegistration bcr, string route = "self")
        {
            return bcr
                .ListeningEvents(typeof(AssetCreatedEvent), typeof(AssetUpdatedEvent))
                    .From(BoundedContext.Name).On(route)
                    .WithProjection(typeof(AssetsProjection), BoundedContext.Name)
                .ListeningEvents(typeof(AssetPairCreatedEvent), typeof(AssetPairUpdatedEvent))
                    .From(BoundedContext.Name).On(route)
                    .WithProjection(typeof(AssetPairsProjection), BoundedContext.Name);
        }
    }
}
