namespace Lykke.Service.Assets.Requests.v2.AssetConditions
{
    /// <summary>
    /// Represents an edited properties of default (wildcard) asset condition.
    /// </summary>
    public class EditAssetDefaultConditionModel
    {
        /// <summary>
        /// Indicated that specified asset available to client.
        /// </summary>
        public bool? AvailableToClient { get; set; }
        
        /// <summary>
        /// The regulation for specified asset.
        /// </summary>
        public string Regulation { get; set; }
    }
}
