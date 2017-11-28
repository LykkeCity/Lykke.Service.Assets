using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetConditions : IAssetConditions
    {
        public AssetConditions(string asset)
        {
            Asset = asset;
        }

        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }

        public void Apply(IAssetConditions assetConditions)
        {
            if (assetConditions.Asset == Asset)
            {
                if (assetConditions.AvailableToClient.HasValue) AvailableToClient = assetConditions.AvailableToClient;
            }
        }
    }
}
