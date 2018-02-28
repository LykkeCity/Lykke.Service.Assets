using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetDefaultConditionEntity : TableEntity
    {
        public AssetDefaultConditionEntity()
        {
        }

        public AssetDefaultConditionEntity(string partitionKey, string rowKey, string layerId)
            : base(partitionKey, rowKey)
        {
            Layer = layerId;
        }

        public string Layer { get; set; }
        public bool? AvailableToClient { get; set; }
        public bool? IsTradable { get; set; }
        public bool? BankCardsDepositEnabled { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public string Regulation { get; set; }
    }
}
