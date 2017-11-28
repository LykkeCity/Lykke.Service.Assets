using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionLayerEntity : TableEntity
    {
        public string Id => RowKey;
        public decimal Priority { get; set; }
        public string Description { get; set; }
    }
}
