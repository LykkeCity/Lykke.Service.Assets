using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetCondition : IAssetCondition
    {
        public AssetCondition(string asset)
        {
            Asset = asset;
        }

        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }

        public void Apply(IAssetCondition assetCondition)
        {
            if (assetCondition.Asset != Asset)
                return;

            if (assetCondition.AvailableToClient.HasValue)
                AvailableToClient = assetCondition.AvailableToClient;

            if (!string.IsNullOrEmpty(assetCondition.Regulation))
                Regulation = assetCondition.Regulation;
        }
    }
}
