﻿using System;
using System.Net.Http;

namespace Lykke.Service.Assets.Client.Custom
{
    /// <summary>
    /// Itended to use in <see cref="ServiceCollectionExtensions.UseAssetsClient"/>
    /// </summary>
    public class AssetServiceSettings
    {
        public Uri BaseUri { get; set; }
        public TimeSpan AssetsCacheExpirationPeriod { get; set; }
        public TimeSpan AssetPairsCacheExpirationPeriod { get; set; }
        public DelegatingHandler[] Handlers { get; set; }

        public static AssetServiceSettings Create(Uri baseUri, TimeSpan cacheExpirationPeriod)
        {
            return new AssetServiceSettings
            {
                BaseUri = baseUri,
                AssetsCacheExpirationPeriod = cacheExpirationPeriod,
                AssetPairsCacheExpirationPeriod = cacheExpirationPeriod
            };
        }

        public static AssetServiceSettings Create(AssetsSettings settings, TimeSpan cacheExpirationPeriod)
        {
            return new AssetServiceSettings
            {
                BaseUri = new Uri(settings.ServiceUrl),
                AssetsCacheExpirationPeriod = cacheExpirationPeriod,
                AssetPairsCacheExpirationPeriod = cacheExpirationPeriod
            };
        }
    }
}