using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionLayerEntity : TableEntity
    {
        public AssetConditionLayerEntity()
        {
        }

        public AssetConditionLayerEntity(string partitionKey, string rowKey, decimal priority, string description,
            bool? clientsCanCashInViaBankCards, bool? swiftDepositEnabled) : base(partitionKey, rowKey)
        {
            Apply(priority, description, clientsCanCashInViaBankCards, swiftDepositEnabled);
        }

        public string Id => RowKey;
        public double Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }

        public AssetConditionLayerEntity Apply(decimal priority, string description,
            bool? clientsCanCashInViaBankCards, bool? swiftDepositEnabled)
        {
            Priority = (double)priority;
            Description = description;
            ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards;
            SwiftDepositEnabled = swiftDepositEnabled;
            return this;
        }
    }
}
