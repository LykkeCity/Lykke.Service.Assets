using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Antares.Service.Assets.Client.Models
{
    public class WatchListDto : IWatchList
    {
        public List<string> AssetIds { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool ReadOnly { get; set; }

        IEnumerable<string> IWatchList.AssetIds => AssetIds;
    }
}
