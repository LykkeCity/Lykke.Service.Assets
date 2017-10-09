using System.Collections.Generic;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class PredefinedWatchListEntity : TableEntity
    {
        public IEnumerable<string> AssetIds { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public int Order { get; set; }

        public bool ReadOnly => false;
    }
}