using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetAttributeEntity : TableEntity, IAssetAttribute
    {
        public string AssetId
        {
            get => PartitionKey;
            set => PartitionKey = value;
        }
        
        public string Key
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string Value
        {
            get;
            set;
        }
    }
}