using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetAttributeEntity : TableEntity, IAssetAttribute
    {
        public string AssetId => PartitionKey;

        public string Key => RowKey;

        public string Value { get; set; }
    }
}