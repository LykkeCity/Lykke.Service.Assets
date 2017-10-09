using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class MarginAssetPairEntity : TableEntity, IMarginAssetPair
    {
        public int Accuracy { get; set; }

        public string BaseAssetId { get; set; }

        public string Id { get; set; }

        public int InvertedAccuracy { get; set; }

        public string Name { get; set; }

        public string QuotingAssetId { get; set; }
    }
}