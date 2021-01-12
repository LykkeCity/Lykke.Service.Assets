using System;
using Autofac;

namespace Antares.Service.Assets.Client
{
    public static class ContainerBuilderExtenions
    {
        /// <summary>
        /// Register IAssetsServiceClient in the builder
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="myNoSqlServerReaderHostPort"></param>
        /// <param name="assetServiceHttpApiUrl"></param>
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
    }
}
