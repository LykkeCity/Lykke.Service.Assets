using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetAttribute : IAssetAttribute
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}