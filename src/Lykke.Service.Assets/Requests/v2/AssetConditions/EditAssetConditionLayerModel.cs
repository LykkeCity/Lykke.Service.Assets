using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Attributes;

namespace Lykke.Service.Assets.Requests.v2.AssetConditions
{
    /// <summary>
    /// Represents an edited properties of asset default condition layer.
    /// </summary>
    public class EditAssetConditionLayerModel
    {
        /// <summary>
        /// The layer id.
        /// </summary>
        [Required]
        [KeyFormat]
        public string Id { get; set; }
        
        /// <summary>
        /// The asset condition layer priority.
        /// </summary>
        [Required]
        public decimal Priority { get; set; }
        
        /// <summary>
        /// The asset condition layer description.
        /// </summary>
        public string Description { get; set; }
        
        /// <summary>
        /// The client ability to cash in via bank cards.
        /// </summary>
        public bool? ClientsCanCashInViaBankCards { get; set; }
        
        /// <summary>
        /// The client ability to swift deposit. 
        /// </summary>
        public bool? SwiftDepositEnabled { get; set; }
    }
}
