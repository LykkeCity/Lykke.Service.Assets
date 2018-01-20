namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetPair
    {
        int Accuracy { get; }

        string BaseAssetId { get; }

        string Id { get; }

        int InvertedAccuracy { get; }

        bool IsDisabled { get; }

        string Name { get; }

        string QuotingAssetId { get; }

        string Source { get; }

        string Source2 { get; }

        double MinVolume { get; }

        double MinInvertedVolume { get; }
    }
}
