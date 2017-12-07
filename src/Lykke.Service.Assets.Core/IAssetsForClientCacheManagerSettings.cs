using System;

namespace Lykke.Service.Assets.Core
{
    public interface IAssetsForClientCacheManagerSettings
    {
        string InstanceName { get; }
        TimeSpan AssetsForClientCacheTimeSpan { get; }
    }
}
