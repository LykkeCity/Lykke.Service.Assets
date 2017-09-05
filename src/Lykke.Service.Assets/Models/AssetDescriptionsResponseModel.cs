using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class AssetDescriptionsResponseModel
    {
        public IEnumerable<AssetExtendedInfo> Descriptions { get; set; }
        public ErrorResponse errorResponse { get; set; }

        public static AssetDescriptionsResponseModel Create(IEnumerable<AssetExtendedInfo> descriptions)
        {
            return new AssetDescriptionsResponseModel
            {
                Descriptions = descriptions
            };

        }

        public static AssetDescriptionsResponseModel Create(ErrorResponse error)
        {
            return new AssetDescriptionsResponseModel
            {
                errorResponse = error
            };

        }
    }

    public class GetAssetDescriptionsRequestModel
    {
        public string[] Ids { get; set; }
    }
}
