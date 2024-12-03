using System;
using System.Net.Http;

// ReSharper disable once CheckNamespace
namespace Lykke.Service.Assets.Client
{
    public partial class AssetsServiceHttp
    {
        /// <inheritdoc />
        /// <summary>
        /// Should be used to prevent memory leak in RetryPolicy
        /// </summary>
        public AssetsServiceHttp(Uri baseUri, HttpClient client) : base(client)
        {
            Initialize();

            BaseUri = baseUri ?? throw new ArgumentNullException("baseUri");
        }
    }
}
