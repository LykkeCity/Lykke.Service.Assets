using Autofac;
using JetBrains.Annotations;
using Lykke.Service.Assets.Client.Projections;
using Lykke.Service.Assets.Client.ReadModels;
using System;
using System.Net.Http;

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
        /// <param name="registerDefaultAssetsReadModel">Whether to register the default in-memory assets and asset-pairs read model.</param>
        [UsedImplicitly]
        public static void RegisterAssetsClient(this ContainerBuilder builder, string serviceUrl, bool registerDefaultAssetsReadModel = true)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.Register(x => new AssetsService(new Uri(serviceUrl), new HttpClient()))
                .As<IAssetsService>()
                .SingleInstance();

            if (registerDefaultAssetsReadModel)
            {
                builder.RegisterDefaultAssetsReadModel();
            }
        }

        /// <summary>
        /// Register the default in-memory assets and asset-pairs read model.
        /// </summary>
        /// <param name="builder">The container builder for adding the services to.</param>
        [UsedImplicitly]
        private static void RegisterDefaultAssetsReadModel(this ContainerBuilder builder)
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
