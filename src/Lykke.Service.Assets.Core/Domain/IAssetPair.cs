namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetPair
    {
        int Accuracy { get; set; }

        string BaseAssetId { get; set; }

        string Id { get; set; }

        int InvertedAccuracy { get; set; }

        bool IsDisabled { get; set; }

        string Name { get; set; }

        string QuotingAssetId { get; set; }

        string Source { get; set; }

        string Source2 { get; set; }
    }
}