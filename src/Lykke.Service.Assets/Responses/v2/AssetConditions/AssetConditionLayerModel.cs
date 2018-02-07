using System.Collections.Generic;
using Lykke.Service.Assets.Attributes;

namespace Lykke.Service.Assets.Responses.v2.AssetConditions
{
    /// <summary>
    /// Represents an asset conditon layer.
    /// </summary>
    public class AssetConditionLayerModel
    {
        /// <summary>
        /// Initializes a new instance of <see cref="AssetConditionLayerModel"/>.
        /// </summary>
        public AssetConditionLayerModel()
        {
            AssetConditions = new List<AssetConditionModel>();
        }

        /// <summary>
        /// The asset condition layer id.
        /// </summary>
        [KeyFormat]
        public string Id { get; set; }
        
        /// <summary>
        /// The asset condition layer priority.
        /// </summary>
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
        
        /// <summary>
        /// The collection of asset conditions for layer. 
        /// </summary>
        public List<AssetConditionModel> AssetConditions { get; set; }
        
        /// <summary>
        /// The wildcard asset condition. It applies to all asset not included in <see cref="AssetConditions"/>. 
        /// </summary>
        public AssetDefaultConditionModel DefaultCondition { get; set; }
    }
}
