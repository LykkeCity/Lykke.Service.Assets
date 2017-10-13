using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetExtendedInfo : IAssetExtendedInfo
    {
        public string AssetClass { get; set; }

        public string AssetDescriptionUrl { get; set; }

        public string Description { get; set; }

        public string FullName { get; set; }

        public string Id { get; set; }

        public string MarketCapitalization { get; set; }

        public string NumberOfCoins { get; set; }

        public int PopIndex { get; set; }
    }
}