using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Assets.Responses.v2
{
    public class AssetConditionSettings
    {
        [Required]
        public bool AvailableToClient { get; set; }

        [Required]
        public string Regulation { get; set; }
    }
}
