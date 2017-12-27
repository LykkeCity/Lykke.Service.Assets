using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetConditionLayerSettingsDto : IAssetConditionLayerSettings
    {
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
    }
}
