using System.Collections.Generic;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IWatchList
    {
        IEnumerable<string> AssetIds { get; set; }

        string Id { get; set; }

        string Name { get; set; }

        int Order { get; set; }

        bool ReadOnly { get; set; }
    }
}