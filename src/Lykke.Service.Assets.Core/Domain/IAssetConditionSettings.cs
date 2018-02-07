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
        /// The regulation associated with asset. 
        /// This setting helps to determine which client profile type used for asset.
        /// </summary>
        string Regulation { get; }
    }
}
