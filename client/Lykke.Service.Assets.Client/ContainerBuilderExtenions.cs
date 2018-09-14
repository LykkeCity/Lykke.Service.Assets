using Autofac;
using JetBrains.Annotations;
using System;
using System.Net.Http;
using Lykke.Service.Assets.Client.Projections;
using Lykke.Service.Assets.Client.ReadModels;

namespace Lykke.Service.Assets.Client
{
    /// <summary>
    /// Service registration for client asset services.
    /// </summary>
    [UsedImplicitly]
    public static class ContainerBuilderExtenions
    {
        /// <summary>
        /// Register the asset services.
        /// </summary>
        /// <param name="builder">The container builder for adding the services to.</param>
        /// <param name="serviceUrl">Service endpoint URL.</param>
        [UsedImplicitly]
        public static void RegisterAssetsClient(this ContainerBuilder builder, string serviceUrl)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.Register(x => new AssetsService(new Uri(serviceUrl), new HttpClient()))
                .As<IAssetsService>()
                .SingleInstance();

            builder.RegisterDefaultAssetsReadModel();
        }

        /// <summary>
        /// Register the default in-memory assets and asset-pairs read model.
        /// </summary>
        /// <param name="builder">The container builder for adding the services to.</param>
        [UsedImplicitly]
        public static void RegisterDefaultAssetsReadModel(this ContainerBuilder builder)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));

            builder.RegisterType<AssetsReadModel>()
                .As<IAssetsReadModel>()
                .As<IStartable>()
                .AutoActivate();
            builder.RegisterType<AssetPairsReadModel>()
                .As<IAssetPairsReadModel>()
                .As<IStartable>()
                .AutoActivate();

            builder.RegisterType<AssetsProjection>();
            builder.RegisterType<AssetPairsProjection>();
        }
    }
}
