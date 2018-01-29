using System.Collections.Generic;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetDefaultConditionLayer : IAssetConditionLayerSettings
    {
        string Id { get; }

        IReadOnlyList<IAssetCondition> AssetConditions { get; }
    }
}
