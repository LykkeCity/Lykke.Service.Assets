using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionLayerEntity : TableEntity
    {
        public AssetConditionLayerEntity()
        {
        }

        public AssetConditionLayerEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public string Id => RowKey;
        public double Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
    }
}
