using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionLayerEntity : TableEntity
    {
        public AssetConditionLayerEntity()
        {
        }

        public AssetConditionLayerEntity(string partitionKey, string rowKey, decimal priority, string description) : base(partitionKey, rowKey)
        {
            Priority = (double)priority;
            Description = description;
        }

        public string Id => RowKey;
        public double Priority { get; set; }
        public string Description { get; set; }
    }
}
