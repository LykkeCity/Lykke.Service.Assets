using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Assets.Requests.v2.AssetConditions
{
    /// <summary>
    /// Represents an edited properties of asset condition.
    /// </summary>
    public class EditAssetConditionModel
    {
        /// <summary>
        /// The asset id.
        /// </summary>
        [Required]
        public string Asset { get; set; }
        
        /// <summary>
        /// Indicated that specified asset available to client.
        /// </summary>
        public bool? AvailableToClient { get; set; }

        /// <summary>
        /// Indicates that assets is tradable.
        /// </summary>
        public bool? IsTradable { get; set; }

        /// <summary>
        /// Indicates that bank cards deposit enabled for asset.
        /// </summary>
        public bool? BankCardsDepositEnabled { get; set; }

        /// <summary>
        /// Indicates that swift deposit enabled for asset.
        /// </summary>
        public bool? SwiftDepositEnabled { get; set; }

        /// <summary>
        /// The regulation for specified asset.
        /// </summary>
        public string Regulation { get; set; }
    }
}
