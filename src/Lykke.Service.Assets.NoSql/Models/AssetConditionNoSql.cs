using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models
{
    public class AssetConditionNoSql : MyNoSqlDbEntity
    {
        public const string TableName = "antares.asset.asset-condition-by-clients";

        public static string GeneratePartitionKey(string clientId) => clientId;
        public static string GenerateRowKey() => "--AssetCondition--";
        
        
        public string ClientId { get; set; }

        public List<AssetConditionModel> AssetConditions { get; set; }

        public static AssetConditionNoSql Create(string clientId, IEnumerable<IAssetCondition> assetConditions)
        {
            var item = new AssetConditionNoSql()
            {
                PartitionKey = GeneratePartitionKey(clientId),
                RowKey = GenerateRowKey(),
                ClientId = clientId,
                AssetConditions = assetConditions.Select(AssetConditionModel.Create).ToList()
            };
            return item;
        }



        public class AssetConditionModel : IAssetCondition
        {
            public bool? AvailableToClient { get; set; }
            public bool? IsTradable { get; set; }
            public bool? BankCardsDepositEnabled { get; set; }
            public bool? SwiftDepositEnabled { get; set; }
            public string Regulation { get; set; }
            public string Asset { get; set; }

            public static AssetConditionModel Create(IAssetCondition assetCondition)
            {
                return new AssetConditionModel()
                {
                    IsTradable = assetCondition.IsTradable,
                    Asset = assetCondition.Asset,
                    BankCardsDepositEnabled = assetCondition.BankCardsDepositEnabled,
                    SwiftDepositEnabled = assetCondition.SwiftDepositEnabled,
                    AvailableToClient = assetCondition.AvailableToClient,
                    Regulation = assetCondition.Regulation
                };
            }
        }
    }
}
