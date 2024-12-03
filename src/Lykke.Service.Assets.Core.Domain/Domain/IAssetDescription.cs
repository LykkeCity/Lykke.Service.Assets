namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetDescription
    {
        string AssetClass { get; }

        string AssetDescriptionUrl { get; }

        string AssetId { get; }

        string Description { get; }

        string FullName { get; }

        string Id { get; }

        string IssuerName { get; }

        string MarketCapitalization { get; }

        string NumberOfCoins { get; }

        int PopIndex { get; }
    }
}