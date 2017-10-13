using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class WatchList : IWatchList
    {
        public IEnumerable<string> AssetIds { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool ReadOnly { get; set; }
    }
}