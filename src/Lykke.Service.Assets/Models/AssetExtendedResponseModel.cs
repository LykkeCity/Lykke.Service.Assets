using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Models
{
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
        public IAsset Asset { get; set; }
        public IAssetDescription Description { get; set; }
        public IAssetCategory Category { get; set; }
        public IEnumerable<IAssetAttributesKeyValue> Attributes { get; set; }

        public static AssetExtended Create(IAsset asset, IAssetDescription description, IAssetCategory category, IEnumerable<IAssetAttributesKeyValue> attributes)
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
