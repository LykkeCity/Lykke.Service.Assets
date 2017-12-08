using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetConditionDto : IAssetCondition
    {
        public AssetConditionDto(string asset, bool? availableToClient)
        {
            Asset = asset;
            AvailableToClient = availableToClient;
        }

        public AssetConditionDto()
        {
        }

        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
