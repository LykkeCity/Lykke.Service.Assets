using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetConditionLayerLinkClientEntity : TableEntity
    {
        public AssetConditionLayerLinkClientEntity()
        {
        }

        public AssetConditionLayerLinkClientEntity(string partitionKey, string rowKey) : base(partitionKey, rowKey)
        {
        }

        public string LayerId => RowKey;
        public string ClientId => PartitionKey;
    }
}