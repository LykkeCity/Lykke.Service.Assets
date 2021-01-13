using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models
{
    public class AssetExtendedInfoNoSql: MyNoSqlDbEntity
    {
        public const string TableName = "antares.asset.asset-extended-info";

        public const string DefaultInfoPartitionKey = "--default--";
        public const string DefaultInfoRowKey = "--default--";

        public static string GeneratePartitionKey() => "AssetExtendedInfo";
        public static string GenerateRowKey(string assetId) => assetId;

        public AssetExtendedInfo ExtendedInfo { get; set; }

        public static AssetExtendedInfoNoSql Create(IAssetExtendedInfo source)
        {
            var item = new AssetExtendedInfoNoSql()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(source.Id),
                ExtendedInfo = AssetExtendedInfo.Create(source)
            };
            return item;
        }

        public class AssetExtendedInfo : IAssetExtendedInfo
        {
            public string AssetClass { get; set; }
            public string AssetDescriptionUrl { get; set; }
            public string Description { get; set; }
            public string FullName { get; set; }
            public string Id { get; set; }
            public string MarketCapitalization { get; set; }
            public string NumberOfCoins { get; set; }
            public int PopIndex { get; set; }

            public static AssetExtendedInfo Create(IAssetExtendedInfo source)
            {
                var item = new AssetExtendedInfo()
                {
                    Id = source.Id,
                    AssetClass = source.AssetClass,
                    AssetDescriptionUrl = source.AssetDescriptionUrl,
                    Description = source.Description,
                    FullName = source.FullName,
                    MarketCapitalization = source.MarketCapitalization,
                    NumberOfCoins = source.NumberOfCoins,
                    PopIndex = source.PopIndex
                };

                return item;
            }
        }
    }
}
