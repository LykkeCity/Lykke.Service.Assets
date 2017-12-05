using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionEntity : TableEntity, IAssetCondition
    {
        public AssetConditionEntity(string partitionKey, string rowKey, 
            string layer, string asset, bool? availableToClient) : base(partitionKey, rowKey)
        {
            Layer = layer;
            Asset = asset;
            AvailableToClient = availableToClient;
        }

        public AssetConditionEntity()
        {
        }

        public string Layer { get; set; }
        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }
    }
}
