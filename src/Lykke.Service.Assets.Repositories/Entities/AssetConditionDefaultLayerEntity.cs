using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionDefaultLayerEntity : TableEntity, IAssetConditionDefaultLayer
    {
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
