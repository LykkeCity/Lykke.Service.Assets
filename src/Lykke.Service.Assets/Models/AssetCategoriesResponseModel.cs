using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class AssetCategoriesResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IosIconUrl { get; set; }
        public string AndroidIconUrl { get; set; }
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
