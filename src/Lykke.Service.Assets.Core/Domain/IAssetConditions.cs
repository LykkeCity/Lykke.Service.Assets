namespace Lykke.Service.Assets.Core.Domain
{
    /// <summary>
    /// Set of settings for the asset, rules\conditions of use of the asset.
    /// These conditions can be redefined at different levels, so everything is not mandatory.
    /// </summary>
    public interface IAssetConditions
    {
        /// <summary>
        /// Asset id for this settings, conditions
        /// </summary>
        string Asset { get; }

        /// <summary>
        /// Asset is avariable to the client. 
        /// This setting can enable or disable asset for the client.
        /// </summary>
        bool? AvailableToClient { get; }
    }
}
