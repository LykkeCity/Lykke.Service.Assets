using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models
{
    public class WatchListCustomNoSql : MyNoSqlDbEntity, IWatchList
    {
        public const string TableNameCustomWatchList = "antares.asset.custom-watch-list";

        public static string GeneratePartitionKey(string clientId) => clientId;
        public static string GenerateRowKey(string watchListId) => watchListId;

        public string ClientId { get; set; }

        public List<string> AssetIds { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public int Order { get; set; }
        public bool ReadOnly { get; set; }

        IEnumerable<string> IWatchList.AssetIds => AssetIds;

        public static WatchListCustomNoSql Create(string clientId, IWatchList watchList)
        {
            return new WatchListCustomNoSql()
            {
                PartitionKey = GeneratePartitionKey(clientId),
                RowKey = GenerateRowKey(watchList.Id),
                Id = watchList.Id,
                Name = watchList.Name,
                Order = watchList.Order,
                ClientId = clientId,
                ReadOnly = watchList.ReadOnly,
                AssetIds = watchList.AssetIds.ToList()
            };
        }
    }
}
