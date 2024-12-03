using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models
{
    public class AssetPairNoSql: MyNoSqlDbEntity
    {
        public const string TableName = "antares_asset_asset-pairs";

        public static string GeneratePartitionKey() => "AssetPair";
        public static string GenerateRowKey(string assetPairId) => assetPairId;

        public AssetPairModel AssetPair { get; set; }

        public static AssetPairNoSql Create(IAssetPair source)
        {
            var item = new AssetPairNoSql()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(source.Id),
                AssetPair = AssetPairModel.Create(source)
            };
            return item;
        }

        public class AssetPairModel : IAssetPair
        {
            public static AssetPairModel Create(IAssetPair source)
            {
                var item = new AssetPairModel()
                {
                    Id = source.Id,
                    Name = source.Name,
                    Accuracy = source.Accuracy,
                    IsDisabled = source.IsDisabled,
                    BaseAssetId = source.BaseAssetId,
                    ExchangeId = source.ExchangeId,
                    InvertedAccuracy = source.InvertedAccuracy,
                    MinInvertedVolume = source.MinInvertedVolume,
                    MinVolume = source.MinVolume,
                    QuotingAssetId = source.QuotingAssetId,
                    Source = source.Source,
                    Source2 = source.Source2
                };

                return item;
            }

            public int Accuracy { get; set; }
            public string BaseAssetId { get; set; }
            public string Id { get; set; }
            public int InvertedAccuracy { get; set; }
            public bool IsDisabled { get; set; }
            public string Name { get; set; }
            public string QuotingAssetId { get; set; }
            public string Source { get; set; }
            public string Source2 { get; set; }
            public double MinVolume { get; set; }
            public double MinInvertedVolume { get; set; }
            public string ExchangeId { get; set; }
        }
    }
}
