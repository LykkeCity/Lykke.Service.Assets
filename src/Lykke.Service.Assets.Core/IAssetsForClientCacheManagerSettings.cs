using System;

namespace Lykke.Service.Assets.Core
{
    public interface IAssetsForClientCacheManagerSettings
    {
        string AssetsForClientCacheKeyPattern { get; set; }
        TimeSpan AssetsForClientCacheTimeSpan { get; set; }
    }
}
