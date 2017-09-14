using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class AssetCategoriesResponseModel
    {
        public IEnumerable<IAssetCategory> Categories { get; set; }
        public ErrorResponse errorResponse { get; set; }

        public static AssetCategoriesResponseModel Create(IEnumerable<IAssetCategory> categories)
        {
            return new AssetCategoriesResponseModel
            {
                Categories = categories
            };

        }

        public static AssetCategoriesResponseModel Create(ErrorResponse error)
        {
            return new AssetCategoriesResponseModel
            {
                errorResponse = error
            };

        }
    }
}

