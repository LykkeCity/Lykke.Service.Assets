using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Assets.Requests.v2.AssetConditions
{
    public class EditAssetDefaultConditionLayerModel
    {
        [Required]
        public bool ClientsCanCashInViaBankCards { get; set; }

        [Required]
        public bool SwiftDepositEnabled { get; set; }
    }
}
