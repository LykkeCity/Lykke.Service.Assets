using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetConditionSettingsDto : IAssetConditionSettings
    {
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
