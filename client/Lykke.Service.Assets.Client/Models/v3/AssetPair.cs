namespace Lykke.Service.Assets.Client.Models.v3
{
    /// <summary>
    /// The main model for the AssetPair.
    /// </summary>
    public class AssetPair
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public int Accuracy { get; set; }
        public int InvertedAccuracy { get; set; }
        public string BaseAssetId { get; set; }
        public string QuotingAssetId { get; set; }
        public decimal MinVolume { get; set; }
        public decimal MinInvertedVolume { get; set; }
        public bool IsDisabled { get; set; }
        public string ExchangeId { get; set; }
    }
}
