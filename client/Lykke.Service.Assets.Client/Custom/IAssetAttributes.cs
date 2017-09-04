using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Client.Custom
{
    public interface IAssetAttributes
    {
        string AssetId { get; set; }
        IList<Lykke.Service.Assets.Client.Models.IAssetAttributesKeyValue> Attributes { get; set; }
    }
}
