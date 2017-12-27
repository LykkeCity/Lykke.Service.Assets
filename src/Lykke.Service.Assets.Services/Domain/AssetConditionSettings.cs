using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetConditionSettings : IAssetConditionSettings
    {
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
