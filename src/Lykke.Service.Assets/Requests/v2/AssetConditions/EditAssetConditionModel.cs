using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Assets.Requests.v2.AssetConditions
{
    public class EditAssetConditionModel
    {
        [Required]
        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
