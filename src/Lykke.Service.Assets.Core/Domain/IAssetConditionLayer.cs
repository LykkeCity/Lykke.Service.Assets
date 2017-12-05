using System.Collections.Generic;

namespace Lykke.Service.Assets.Core.Domain
{
    /// <summary>
    /// Layer of conditions for the use of assets.
    /// The layer groups the condition of use of several assets.
    /// Also describes the rule for applying these settings and their purpose.
    /// </summary>
    public interface IAssetConditionLayer : IAssetConditionLayerSettings
    {
        /// <summary>
        /// Identity of this asset conditions layer.
        /// </summary>
        string Id { get; }

        /// <summary>
        /// Dictionary with asset condition
        /// Key - asset
        /// Valuet - asset conditions
        /// </summary>
        IReadOnlyDictionary<string, IAssetCondition> AssetConditions { get; }

        /// <summary>
        /// Prioryty of layer.
        /// In the process of calculating the setting, the system searches for the installed property, 
        /// scan all the layers, assigned to the client, in order of decreasing priority.
        /// </summary>
        decimal Priority { get; }

        /// <summary>
        /// Desctiotion to this layer, form make node or comment about purpose of this layer.
        /// Can be use with BackOffice
        /// </summary>
        string Description { get; }

        
    }
}
