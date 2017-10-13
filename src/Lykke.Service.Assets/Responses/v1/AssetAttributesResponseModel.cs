using System;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.v1
{
    [Obsolete]
    public class AssetAttributesResponseModel
    {
        public string AssetId;
        public IAssetAttribute[] Attributes { get; set; }

        public static AssetAttributesResponseModel Create(string assetId, IAssetAttribute[] assetAttributes)
        {
            return new AssetAttributesResponseModel
            {
                Attributes = assetAttributes,
                AssetId = assetId
            };
        }
    }
}
