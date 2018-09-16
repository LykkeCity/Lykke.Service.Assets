using Lykke.Service.Assets.Client.Models.v3;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.ReadModels
{
    /// <summary>
    /// Useful, but non-optimized extension for IAssetsReadModelRepository and IAssetPairsReadModelRepository.
    /// </summary>
    public static class AssetsReadModelExtensions
    {
        /// <summary>
        /// Get the asset if it is enabled. Returns null, if the asset is disabled.
        /// </summary>
        public static Asset TryGetIfEnabled(this IAssetsReadModelRepository readModelRepository, string id)
        {
            var asset = readModelRepository.TryGet(id);
            return asset != null && !asset.IsDisabled ? asset : null;
        }

        /// <summary>
        /// Get the asset-pair if it is enabled. Returns null, if the asset-pair is disabled.
        /// </summary>
        public static AssetPair TryGetIfEnabled(this IAssetPairsReadModelRepository readModelRepository, string id)
        {
            var assetPair = readModelRepository.TryGet(id);
            return assetPair != null && !assetPair.IsDisabled ? assetPair : null;
        }

        /// <summary>
        /// Get all enabled assets.
        /// </summary>
        public static IReadOnlyCollection<Asset> GetAllEnabled(this IAssetsReadModelRepository readModelRepository)
        {
            var assets = readModelRepository.GetAll();
            return assets.Where(x => !x.IsDisabled).ToArray();
        }

        /// <summary>
        /// Get all enabled asset-pairs.
        /// </summary>
        public static IReadOnlyCollection<AssetPair> GetAllEnabled(this IAssetPairsReadModelRepository readModelRepository, string id)
        {
            var assetPairs = readModelRepository.GetAll();
            return assetPairs.Where(x => !x.IsDisabled).ToArray();
        }
    }
}
