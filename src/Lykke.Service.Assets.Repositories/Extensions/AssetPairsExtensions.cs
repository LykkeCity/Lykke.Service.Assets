using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.Extensions
{
    public static class AssetPairsExtensions
    {
        public static string GetFirstAssetId(this IEnumerable<IAsset> assets)
        {
            return assets.OrderBy(x => x.DefaultOrder).First().Id;
        }

        public static IEnumerable<IAssetPair> WhichHaveAssets(this IEnumerable<IAssetPair> src, params string[] assetIds)
        {
            return src.Where(x => assetIds.Contains(x.BaseAssetId) || assetIds.Contains(x.QuotingAssetId));
        }

        public static IEnumerable<IAssetPair> WhichConsistsOfAssets(this IEnumerable<IAssetPair> src, params string[] assetIds)
        {
            return src.Where(x => assetIds.Contains(x.BaseAssetId) && assetIds.Contains(x.QuotingAssetId));
        }
    }
}