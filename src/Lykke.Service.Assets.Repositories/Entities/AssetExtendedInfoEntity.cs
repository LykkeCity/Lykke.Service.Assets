using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetExtendedInfoEntity : TableEntity, IAssetExtendedInfo
    {
        public string AssetClass { get; set; }

        public string AssetDescriptionUrl { get; set; }

        public string Description { get; set; }

        public string FullName { get; set; }

        public string Id => RowKey;

        public string MarketCapitalization { get; set; }

        public string NumberOfCoins { get; set; }

        public int PopIndex { get; set; }
    }
}