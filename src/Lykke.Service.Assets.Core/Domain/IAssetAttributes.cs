using System.Collections.Generic;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetAttributes
    {
        string AssetId { get; set; }

        IEnumerable<IAssetAttribute> Attributes { get; set; }
    }
}
