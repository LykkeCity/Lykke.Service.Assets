using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetDefaultCondition : IAssetDefaultCondition
    {
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
