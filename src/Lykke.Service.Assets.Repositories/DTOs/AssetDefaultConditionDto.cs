using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetDefaultConditionDto : IAssetDefaultCondition
    {
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
