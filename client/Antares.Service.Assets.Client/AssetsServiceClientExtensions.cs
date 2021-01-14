using System;
using Autofac;
using Lykke.Service.Assets.Client;

namespace Antares.Service.Assets.Client
{
    public static class ContainerBuilderExtenions
    {
        /// <summary>
        /// Register IAssetsServiceClient in the builder
        /// </summary>
        public static void RegisterAssetsHttpClient(this ContainerBuilder builder, string myNoSqlServerReaderHostPort, string assetServiceHttpApiUrl)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(builder));
            if (string.IsNullOrWhiteSpace(myNoSqlServerReaderHostPort))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(myNoSqlServerReaderHostPort));
            if (string.IsNullOrWhiteSpace(assetServiceHttpApiUrl))
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(assetServiceHttpApiUrl));

            builder.RegisterType<AssetsServiceClient>()
                .WithParameter("myNoSqlServerReaderHostPort", myNoSqlServerReaderHostPort)
                .WithParameter("assetServiceHttpApiUrl", assetServiceHttpApiUrl)
                .As<IAssetsServiceClient>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();
        }

        /// <summary>
        /// Register IAssetsServiceClient in the builder
        /// </summary>
        public static void RegisterAssetsHttpClient(this ContainerBuilder builder, AssetServiceSettings serviceSettings)
        {
            if (builder == null)
                throw new ArgumentNullException(nameof(serviceSettings));

            builder.RegisterAssetsHttpClient(serviceSettings.AssetServiceMyNoSqlReaderHostPort, serviceSettings.ServiceUrl);
        }
    }
}
