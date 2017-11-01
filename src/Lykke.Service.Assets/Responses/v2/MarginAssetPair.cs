using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class MarginAssetPair : IMarginAssetPair
    {
        public int Accuracy { get; set; }

        public string BaseAssetId { get; set; }

        public string Id { get; set; }

        public int InvertedAccuracy { get; set; }

        public string Name { get; set; }

        public string QuotingAssetId { get; set; }
    }
}