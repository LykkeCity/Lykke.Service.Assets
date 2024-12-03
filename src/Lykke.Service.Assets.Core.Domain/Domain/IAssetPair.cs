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

        /// <summary>
        /// Minimum volume of Limit or Market orders
        /// </summary>
        double MinVolume { get; }

        /// <summary>
        /// Minimum volume of Limit or Market orders for inverted pair
        /// </summary>
        double MinInvertedVolume { get; }

        /// <summary>
        /// Asset pair can be listed on external exchange
        /// For Lykke exchange value is "null"
        /// </summary>
        string ExchangeId { get; }
    }
}
