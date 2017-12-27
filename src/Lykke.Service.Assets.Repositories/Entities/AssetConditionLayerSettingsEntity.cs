using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionLayerSettingsEntity : TableEntity
    {
        public AssetConditionLayerSettingsEntity()
        {
        }

        public AssetConditionLayerSettingsEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
    }
}
