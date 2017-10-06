using System.Collections.Generic;

namespace Lykke.Service.Assets.Models
{
    public class AssetDescriptionListRequest
    {
        public IEnumerable<string> Ids { get; set; }
    }
}