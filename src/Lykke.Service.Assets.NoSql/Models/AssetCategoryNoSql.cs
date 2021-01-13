using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models
{
    public class AssetCategoryNoSql : MyNoSqlDbEntity
    {
        public const string TableName = "antares.asset.category";

        public static string GeneratePartitionKey() => "category";
        public static string GenerateRowKey(string id) => id;

        public AssetCategory Category { get; set; }

        public static AssetCategoryNoSql Create(IAssetCategory source)
        {
            var item = new AssetCategoryNoSql()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(source.Id),
                Category = AssetCategory.Create(source)
            };
            return item;
        }

        public class AssetCategory : IAssetCategory
        {
            public string AndroidIconUrl { get; set; }
            public string Id { get; set; }
            public string IosIconUrl { get; set; }
            public string Name { get; set; }
            public int SortOrder { get; set; }

            public static AssetCategory Create(IAssetCategory source)
            {
                return new AssetCategory()
                {
                    Id = source.Id,
                    AndroidIconUrl = source.AndroidIconUrl,
                    IosIconUrl = source.IosIconUrl,
                    Name = source.Name,
                    SortOrder = source.SortOrder
                };
            }
        }
    }
}
