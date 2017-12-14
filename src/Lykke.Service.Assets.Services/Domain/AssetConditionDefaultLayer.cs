using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetConditionDefaultLayer : IAssetConditionDefaultLayer
    {
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
