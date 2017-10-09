namespace Lykke.Service.Assets.Core.Domain
{
    public interface IMarginAssetPair
    {
        int Accuracy { get; set; }

        string BaseAssetId { get; set; }

        string Id { get; set; }

        int InvertedAccuracy { get; set; }

        string Name { get; set; }

        string QuotingAssetId { get; set; }
    }
}