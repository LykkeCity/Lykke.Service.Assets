using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models.AssetAttributeModel
{
    public class AssetAttributeNoSql: MyNoSqlDbEntity, IAssetAttribute
    {
        public static string TableName = "antares.asset.attribute";

        public static string GeneratePartitionKey(string assetId) => assetId;
        public static string GenerateRowKey(string key) => key;

        public string AssetId { get; set; }

        public string Key { get; set; }

        public string Value { get; set; }

        public static AssetAttributeNoSql Create(string assetId, string key, string value)
        {
            var item = new AssetAttributeNoSql()
            {
                PartitionKey = GeneratePartitionKey(assetId),
                RowKey = GenerateRowKey(key),
                AssetId = assetId,
                Key = key,
                Value = value
            };
            return item;
        }
    }
}
