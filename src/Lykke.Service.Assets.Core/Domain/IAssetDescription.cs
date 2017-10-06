namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetDescription
    {
        string AssetClass { get; set; }

        string AssetDescriptionUrl { get; set; }

        string AssetId { get; set; }

        string Description { get; set; }

        string FullName { get; set; }

        string Id { get; set; }

        string IssuerName { get; set; }

        string MarketCapitalization { get; set; }

        string NumberOfCoins { get; set; }

        int PopIndex { get; set; }
    }
}