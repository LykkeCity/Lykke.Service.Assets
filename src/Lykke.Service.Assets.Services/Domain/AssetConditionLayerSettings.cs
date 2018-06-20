using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetConditionLayerSettings : IAssetConditionLayerSettings
    {
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }

        public void Apply(IAssetConditionLayerSettings settings)
        {
            if (settings.ClientsCanCashInViaBankCards.HasValue)
                ClientsCanCashInViaBankCards = settings.ClientsCanCashInViaBankCards;

            if (settings.SwiftDepositEnabled.HasValue)
                SwiftDepositEnabled = settings.SwiftDepositEnabled;
        }
    }
}
