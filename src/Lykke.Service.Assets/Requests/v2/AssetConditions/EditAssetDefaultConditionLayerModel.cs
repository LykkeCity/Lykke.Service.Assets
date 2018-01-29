namespace Lykke.Service.Assets.Requests.v2.AssetConditions
{
    /// <summary>
    /// Represents an edited properties of asset default condition layer.
    /// </summary>
    public class EditAssetDefaultConditionLayerModel
    {
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
