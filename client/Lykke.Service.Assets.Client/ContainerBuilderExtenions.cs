using Autofac;
using JetBrains.Annotations;
using System;
using System.Net.Http;

namespace Lykke.Service.Assets.Client
{
    /// <summary>
    /// Service registration for client asset services.
    /// </summary>
    [UsedImplicitly]
    internal static class ContainerBuilderExtenions
    {
        /// <summary>
        /// Register the asset services.
        /// </summary>
        /// <param name="builder">The container builder for adding the services to.</param>
        /// <param name="serviceSettings">Service settings.</param>
        /// <param name="registerDefaultAssetsReadModel">Whether to register the default in-memory assets and asset-pairs read model.
        /// Please call IBoundedContextRegistration.WithAssetsReadModel during configuration of your bounded context.</param>
        [UsedImplicitly]
        public static void RegisterAssetsHttpClientOld(this ContainerBuilder builder, 
            [NotNull] AssetServiceSettings serviceSettings, 
            bool registerDefaultAssetsReadModel = true)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (serviceSettings == null)
                throw new ArgumentNullException(nameof(serviceSettings));

            builder.RegisterAssetsHttpClientOld(serviceSettings.ServiceUrl, registerDefaultAssetsReadModel);
        }

        /// <summary>
        /// Register the asset services.
        /// </summary>
        /// <param name="builder">The container builder for adding the services to.</param>
        /// <param name="serviceUrl">Service endpoint URL.</param>
        /// <param name="registerDefaultAssetsReadModel">Whether to register the default in-memory assets and asset-pairs read model.
        /// Please call IBoundedContextRegistration.WithAssetsReadModel during configuration of your bounded context.</param>
        [UsedImplicitly]
        public static void RegisterAssetsHttpClientOld(this ContainerBuilder builder, string serviceUrl, bool registerDefaultAssetsReadModel = true)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(serviceUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(serviceUrl));

            builder.Register(x => new AssetsServiceHttp(new Uri(serviceUrl), new HttpClient()))
                .As<IAssetsServiceHttp>()
                .SingleInstance();
        }
    }
}
