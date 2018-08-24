﻿using System;

namespace Lykke.Service.Assets.Client
{
    public class AssetServiceSettings
    {
        public TimeSpan AssetsCacheExpirationPeriod { get; set; }

        public TimeSpan AssetPairsCacheExpirationPeriod { get; set; }

        public Uri BaseUri { get; set; }


        public static AssetServiceSettings Create(Uri baseUri, TimeSpan cacheExpirationPeriod)
        {
            return new AssetServiceSettings
            {
                BaseUri                         = baseUri,
                AssetsCacheExpirationPeriod     = cacheExpirationPeriod,
                AssetPairsCacheExpirationPeriod = cacheExpirationPeriod
            };
        }
    }
}
