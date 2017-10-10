using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetAttribute : IAssetAttribute
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Value { get; set; }
    }
}