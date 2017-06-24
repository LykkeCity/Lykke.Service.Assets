using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    [UsedImplicitly]
    public class AssetPairsCacheService : IAssetPairsCacheService
    {
        private Dictionary<string, IAssetPair> _pairs = new Dictionary<string, IAssetPair>();

        public void Update(IEnumerable<IAssetPair> pairs)
        {
            _pairs = pairs.ToDictionary(p => p.Id, p => p);
        }

        public IAssetPair TryGetPair(string assetPairId)
        {
            _pairs.TryGetValue(assetPairId, out IAssetPair pair);

            return pair;
        }

        public IReadOnlyCollection<IAssetPair> GetAll()
        {
            return _pairs.Values;
        }
    }
}