using Lykke.Service.Assets.Core.Domain;
using System.Collections.Generic;

namespace Lykke.Service.Assets.Models
{
    public class AssetAttributes : IAssetAttributes
    {
        public string AssetId { get; set; }

        public IEnumerable<IAssetAttribute> Attributes { get; set; }
        
        public string Id { get; set; }
        

        public static AssetAttributes Create(string assetId, IAssetAttribute[] assetAttributes)
        {
            return new AssetAttributes
            {
                AssetId    = assetId,
                Attributes = assetAttributes
            };
        }
    }
}
