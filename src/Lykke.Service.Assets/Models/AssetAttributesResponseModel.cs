using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class AssetAttributesResponseModel
    {
        public string assetId;
        public IAssetAttributesKeyValue[] Pairs { get; set; }
        public ErrorResponse errorResponse { get; set; }

        public static AssetAttributesResponseModel Create(IAssetAttributes assetAttributes)
        {
            return new AssetAttributesResponseModel
            {
                Pairs = assetAttributes.Attributes.ToArray(),
                assetId = assetAttributes.AssetId
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
