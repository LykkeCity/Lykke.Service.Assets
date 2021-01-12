namespace Lykke.Service.Assets.Core.Domain
{
    public interface IMarginAssetPair
    {
        int Accuracy { get; }

        string BaseAssetId { get; }

        string Id { get; }

        int InvertedAccuracy { get; }

        string Name { get; }

        string QuotingAssetId { get; }
    }
}