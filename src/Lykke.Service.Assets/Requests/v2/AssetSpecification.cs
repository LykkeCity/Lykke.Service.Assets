using System.Collections.Generic;

namespace Lykke.Service.Assets.Requests.V2
{
    public class AssetSpecification
    {
        public IEnumerable<string> Ids { get; set; }

        public bool? IsTradable { get; set; }
    }
}
