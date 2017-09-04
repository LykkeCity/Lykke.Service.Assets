using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class AssetAttributesResponseModel
    {
        public string AssetId;
        public IAssetAttributesKeyValue[] Attributes { get; set; }
        public ErrorResponse errorResponse { get; set; }

        public static AssetAttributesResponseModel Create(string assetId, IAssetAttributesKeyValue[] assetAttributes)
        {
            return new AssetAttributesResponseModel
            {
                Attributes = assetAttributes,
                AssetId = assetId
            };

        }
        public static AssetAttributesResponseModel Create(ErrorResponse error)
        {
            return new AssetAttributesResponseModel
            {
                errorResponse = error
            };

        }
    }
}
