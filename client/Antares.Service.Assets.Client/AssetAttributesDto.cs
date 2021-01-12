using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Antares.Service.Assets.Client
{
    internal class AssetAttributesDto: IAssetAttributes
    {
        public string AssetId { get; set; }
        public List<IAssetAttribute> Attributes { get; set; } 

        IEnumerable<IAssetAttribute> IAssetAttributes.Attributes => Attributes;
    }
}
