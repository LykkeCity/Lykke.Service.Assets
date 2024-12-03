namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetExtendedInfo
    {
        string AssetClass { get; }

        string AssetDescriptionUrl { get; }

        string Description { get; }

        string FullName { get; }

        string Id { get; }

        string MarketCapitalization { get; }

        string NumberOfCoins { get; }

        int PopIndex { get; }
    }
}