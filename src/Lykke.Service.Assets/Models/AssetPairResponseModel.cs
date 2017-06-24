using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetPairResponseModel
    {
        public string Id { get; private set; }
        public string Name { get; private set; }
        public string BaseAssetId { get; private set; }
        public string QuotingAssetId { get; private set; }
        public int Accuracy { get; private set; }
        public int InvertedAccuracy { get; private set; }
        public string Source { get; private set; }
        public string Source2 { get; private set; }
        public bool IsDisabled { get; private set; }

        public static AssetPairResponseModel Create(IAssetPair src)
        {
            return new AssetPairResponseModel
            {
                Id = src.Id,
                Name = src.Name,
                BaseAssetId = src.BaseAssetId,
                QuotingAssetId = src.QuotingAssetId,
                Accuracy = src.Accuracy,
                InvertedAccuracy = src.InvertedAccuracy,
                Source = src.Source,
                Source2 = src.Source2,
                IsDisabled = src.IsDisabled
            };
        }
    }
}