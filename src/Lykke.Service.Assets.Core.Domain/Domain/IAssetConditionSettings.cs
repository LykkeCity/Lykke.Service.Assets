using System;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetConditionSettings
    {
        /// <summary>
        /// Asset is avariable to the client. 
        /// This setting can enable or disable asset for the client.
        /// </summary>
        bool? AvailableToClient { get; }

        /// <summary>
        /// Indicates that assets is tradable.
        /// </summary>
        bool? IsTradable { get; }

        /// <summary>
        /// Indicates that bank cards deposit enabled for asset.
        /// </summary>
        bool? BankCardsDepositEnabled { get; }

        /// <summary>
        /// Indicates that swift deposit enabled for asset.
        /// </summary>
        bool? SwiftDepositEnabled { get; }

        /// <summary>
        /// The regulation associated with asset. 
        /// This setting helps to determine which client profile type used for asset.
        /// </summary>
        [Obsolete("The regulations are deprecated.")]
        string Regulation { get; }
    }
}
