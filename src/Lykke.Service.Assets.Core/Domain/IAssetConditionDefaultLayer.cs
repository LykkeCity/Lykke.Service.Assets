namespace Lykke.Service.Assets.Core.Domain
{
    /// <summary>
    /// Default settings for assets
    /// </summary>
    public interface IAssetConditionDefaultLayer
    {
        /// <summary>
        /// Enable or disable cashin via back card for all assets.
        /// </summary>
        bool? ClientsCanCashInViaBankCards { get; }

        /// <summary>
        /// Enable or disable cashin swift for all assets.
        /// </summary>
        bool? SwiftDepositEnabled { get; }

        /// <summary>
        /// Asset is avariable to the client. 
        /// This setting can enable or disable asset for the client.
        /// </summary>
        bool? AvailableToClient { get; }

        /// <summary>
        /// The regulation associated with asset. 
        /// This setting helps to determine which client profile type used for asset.
        /// </summary>
        string Regulation { get; }
    }
}
