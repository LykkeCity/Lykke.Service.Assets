using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;


namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetPairEntity : TableEntity, IAssetPair
    {
        public int Accuracy { get; set; }

        public string BaseAssetId { get; set; }

        public string Id => RowKey;

        public int InvertedAccuracy { get; set; }

        public bool IsDisabled { get; set; }

        public string Name { get; set; }

        public string QuotingAssetId { get; set; }

        public string Source { get; set; }

        public string Source2 { get; set; }
    }
}