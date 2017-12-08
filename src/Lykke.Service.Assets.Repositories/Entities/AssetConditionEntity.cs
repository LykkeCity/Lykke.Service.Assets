using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetConditionEntity : TableEntity, IAssetCondition
    {
        public AssetConditionEntity()
        {
        }

        public AssetConditionEntity(
            string partitionKey,
            string rowKey,
            string layer,
            string asset,
            bool? availableToClient,
            string regulation)
            : base(partitionKey, rowKey)
        {
            Layer = layer;
            Asset = asset;
            AvailableToClient = availableToClient;
            Regulation = regulation;
        }

        public string Layer { get; set; }
        public string Asset { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
