using System;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.v1
{
    [Obsolete]
    public class AssetCategoriesResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IosIconUrl { get; set; }
        public string AndroidIconUrl { get; set; }
        [Required]
        public int SortOrder { get; set; }

        public static AssetCategoriesResponseModel Create(IAssetCategory src)
        {
            return new AssetCategoriesResponseModel
            {
                Id = src.Id,
                Name = src.Name,
                IosIconUrl = src.IosIconUrl,
                AndroidIconUrl = src.AndroidIconUrl,
                SortOrder = src.SortOrder
            };
        }
    }
}
