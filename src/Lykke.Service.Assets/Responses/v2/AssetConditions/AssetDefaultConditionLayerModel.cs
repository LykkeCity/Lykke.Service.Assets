using System.Collections.Generic;

namespace Lykke.Service.Assets.Responses.v2.AssetConditions
{
    /// <summary>
    /// Represents an asset default condition layer.
    /// </summary>
    public class AssetDefaultConditionLayerModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AssetDefaultConditionLayerModel"/>.
        /// </summary>
        public AssetDefaultConditionLayerModel()
        {
            AssetConditions = new List<AssetConditionModel>();
        }

        /// <summary>
        /// The layer id.
        /// </summary>
        public string Id { get; set; }
        
        /// <summary>
        /// The client ability to cash in via bank cards.
        /// </summary>
        public bool? ClientsCanCashInViaBankCards { get; set; }
        
        /// <summary>
        /// The client ability to swift deposit. 
        /// </summary>
        public bool? SwiftDepositEnabled { get; set; }
        
        /// <summary>
        /// The collection of asset conditions for layer. 
        /// </summary>
        public List<AssetConditionModel> AssetConditions { get; set; }
    }
}
