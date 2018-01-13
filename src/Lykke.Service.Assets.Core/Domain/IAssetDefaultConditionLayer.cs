using System.Collections.Generic;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetDefaultConditionLayer : IAssetConditionLayerSettings
    {
        string Id { get; }

        IList<IAssetCondition> AssetConditions { get; }
    }
}
