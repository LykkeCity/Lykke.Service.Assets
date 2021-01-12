using System.Collections.Generic;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IWatchList
    {
        IEnumerable<string> AssetIds { get; }

        string Id { get; }

        string Name { get; }

        int Order { get; }

        bool ReadOnly { get; }
    }
}