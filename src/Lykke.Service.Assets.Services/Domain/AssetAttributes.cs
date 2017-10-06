using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetAttributes : IAssetAttributes
    {
        public string AssetId { get; set; }

        public IEnumerable<IAssetAttribute> Attributes { get; set; }
    }
}