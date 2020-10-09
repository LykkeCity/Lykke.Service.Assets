using System;

namespace Lykke.Service.Assets.Core
{
    public interface IAssetsForClientCacheManagerSettings
    {
        string Instance { get; }
        TimeSpan AssetsForClientCacheTimeSpan { get; }
    }
}
