using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetConditionDto : IAssetCondition
    {
        public AssetConditionDto()
        {
        }

        public AssetConditionDto(string asset, bool? availableToClient, string regulation)
        {
            Asset = asset;
            AvailableToClient = availableToClient;
            Regulation = regulation;
        }

        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
