using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models
{
    public class WatchListPredefinedNoSql : MyNoSqlDbEntity, IWatchList
    {
        public const string TableNamePredefinedWatchList = "antares.asset.predefined-watch-list";

        public static string GeneratePartitionKey() => "predefined";
        public static string GenerateRowKey(string watchListId) => watchListId;

        public List<string> AssetIds { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool ReadOnly { get; set; }

        IEnumerable<string> IWatchList.AssetIds => AssetIds;

        public static WatchListPredefinedNoSql Create(IWatchList watchList)
        {
            return new WatchListPredefinedNoSql()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(watchList.Id),
                Id = watchList.Id,
                Name = watchList.Name,
                Order = watchList.Order,
                ReadOnly = watchList.ReadOnly,
                AssetIds = watchList.AssetIds.ToList()
            };
        }
    }
}
