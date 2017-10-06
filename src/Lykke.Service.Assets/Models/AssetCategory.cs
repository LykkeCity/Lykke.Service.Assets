using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetCategory : IAssetCategory
    {
        public string AndroidIconUrl { get; set; }

        public string Id { get; set; }

        public string IosIconUrl { get; set; }

        public string Name { get; set; }

        [Required]
        public int SortOrder { get; set; }


        public static AssetCategory Create(IAssetCategory src)
        {
            return new AssetCategory
            {
                AndroidIconUrl = src.AndroidIconUrl,
                Id             = src.Id,
                IosIconUrl     = src.IosIconUrl,
                Name           = src.Name,
                SortOrder      = src.SortOrder
            };
        }
    }
}