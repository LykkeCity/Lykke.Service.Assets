using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Assets.Responses.v2
{
    public class AssetConditionLayerSettings
    {
        [Required]
        public bool ClientsCanCashInViaBankCards { get; set; }

        [Required]
        public bool SwiftDepositEnabled { get; set; }
    }
}
