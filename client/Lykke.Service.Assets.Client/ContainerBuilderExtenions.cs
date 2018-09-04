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
        }
    }
}
