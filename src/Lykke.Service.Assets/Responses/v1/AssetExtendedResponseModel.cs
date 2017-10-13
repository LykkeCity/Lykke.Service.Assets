using System;
using System.Collections.Generic;

namespace Lykke.Service.Assets.Responses.v1
{
    [Obsolete]
    public class AssetExtendedResponseModel 
    {
       public IEnumerable<AssetExtended> Assets { get; set; }

       public static AssetExtendedResponseModel Create(IEnumerable<AssetExtended> assets)
       {
            return new AssetExtendedResponseModel
            {
                Assets = assets
            };            
       }
    }

    public class AssetExtended 
    {
        public AssetResponseModel Asset { get; set; }
        public AssetDescriptionsResponseModel Description { get; set; }
        public AssetCategoriesResponseModel Category { get; set; }
        public AssetAttributesResponseModel Attributes { get; set; }

        public static AssetExtended Create(AssetResponseModel asset, AssetDescriptionsResponseModel description, AssetCategoriesResponseModel category, AssetAttributesResponseModel attributes)
        {
            return new AssetExtended
            {
                Asset = asset,
                Description = description,
                Category = category,
                Attributes = attributes
            };
        }
    }
}
