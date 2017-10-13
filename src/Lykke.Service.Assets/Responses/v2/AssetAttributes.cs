using System.Collections.Generic;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetAttributes
    {
        public string AssetId { get; set; }

        public IEnumerable<AssetAttribute> Attributes { get; set; }
    }
}
