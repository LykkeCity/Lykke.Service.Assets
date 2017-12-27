using System.ComponentModel.DataAnnotations;

namespace Lykke.Service.Assets.Responses.v2
{
    public class AssetConditionDefaultSettings
    {
        [Required]
        public AssetConditionSettings AssetSettings { get; set; }

        [Required]
        public AssetConditionLayerSettings LayerSettings { get; set; }
    }
}
