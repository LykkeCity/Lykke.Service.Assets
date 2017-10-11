using System.Collections.Generic;

namespace Lykke.Service.Assets.Models
{
    public class AssetAttributes
    {
        public string AssetId { get; set; }

        public IEnumerable<AssetAttribute> Attributes { get; set; }
    }
}
