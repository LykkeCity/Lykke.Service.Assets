using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetCategory : IAssetCategory
    {
        public string AndroidIconUrl { get; set; }

        [Required]
        public string Id { get; set; }

        public string IosIconUrl { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int SortOrder { get; set; }
    }
}