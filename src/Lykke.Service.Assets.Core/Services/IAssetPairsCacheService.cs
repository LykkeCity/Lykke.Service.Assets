using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetPairsCacheService
    {
        void Update(IEnumerable<IAssetPair> pairs);
        IAssetPair TryGetPair(string assetPairId);
        IReadOnlyCollection<IAssetPair> GetAll();
    }
}