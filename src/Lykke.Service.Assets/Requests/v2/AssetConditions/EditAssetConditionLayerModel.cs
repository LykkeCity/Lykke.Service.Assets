using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Attributes;

namespace Lykke.Service.Assets.Requests.v2.AssetConditions
{
    public class EditAssetConditionLayerModel
    {
        [Required]
        [KeyFormat]
        public string Id { get; set; }
        [Required]
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
    }
}
