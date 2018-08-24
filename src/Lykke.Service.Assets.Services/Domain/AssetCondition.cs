using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetCondition : IAssetCondition
    {
        public AssetCondition()
        {
        }

        public AssetCondition(string asset)
        {
            Asset = asset;
        }

        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }
        public bool? IsTradable { get; set; }
        public bool? BankCardsDepositEnabled { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public string Regulation { get; set; }

        public void Apply(IAssetConditionSettings assetCondition)
        {
            if (assetCondition.AvailableToClient.HasValue)
                AvailableToClient = assetCondition.AvailableToClient;

            if (!string.IsNullOrEmpty(assetCondition.Regulation))
                Regulation = assetCondition.Regulation;
        }
    }
}
