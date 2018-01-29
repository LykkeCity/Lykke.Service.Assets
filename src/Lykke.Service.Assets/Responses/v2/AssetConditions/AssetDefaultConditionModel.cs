namespace Lykke.Service.Assets.Responses.v2.AssetConditions
{
    /// <summary>
    /// Represents a default (wildcard) asset condition.
    /// </summary>
    public class AssetDefaultConditionModel
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
