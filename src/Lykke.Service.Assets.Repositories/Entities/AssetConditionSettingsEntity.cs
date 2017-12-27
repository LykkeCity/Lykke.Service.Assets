using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionSettingsEntity : TableEntity
    {
        public AssetConditionSettingsEntity()
        {
        }

        public AssetConditionSettingsEntity(string partitionKey, string rowKey)
            : base(partitionKey, rowKey)
        {
        }

        public bool? AvailableToClient { get; set; }

        public string Regulation { get; set; }
    }
}
