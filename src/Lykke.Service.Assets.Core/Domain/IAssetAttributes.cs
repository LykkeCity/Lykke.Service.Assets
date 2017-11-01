using System.Collections.Generic;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetAttributes
    {
        string AssetId { get; }

        IEnumerable<IAssetAttribute> Attributes { get; }
    }
}
