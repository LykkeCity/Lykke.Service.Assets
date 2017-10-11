using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.Models.Extensions
{
    public static class AssetPairExtensions
    {
        //public static async Task<AssetPair> GetAsync(this IAssetPairsRepository assetPairsRepository, string assetId1, string assetId2)
        //{
        //    var assetPairs = await assetPairsRepository.GetAllAsync();
        //    return assetPairs.FirstOrDefault(itm =>
        //        (itm.BaseAssetId == assetId1 && itm.QuotingAssetId == assetId2) ||
        //        (itm.BaseAssetId == assetId2 && itm.QuotingAssetId == assetId1));
        //}

        public static bool IsInverted(this AssetPair assetPair, string targetAsset)
        {
            return assetPair.QuotingAssetId == targetAsset;
        }

        public static int Multiplier(this AssetPair assetPair)
        {
            return (int) Math.Pow(10, assetPair.Accuracy);
        }

        public static AssetPair PairWithAssets(this IEnumerable<AssetPair> src, string assetId1, string assetId2)
        {
            return src.FirstOrDefault(assetPair =>
                assetPair.BaseAssetId == assetId1 && assetPair.QuotingAssetId == assetId2 ||
                assetPair.BaseAssetId == assetId2 && assetPair.QuotingAssetId == assetId1
            );
        }

        public static string RateToString(this double src, AssetPair assetPair)
        {
            var mask = "0." + new string('#', assetPair.Accuracy);
            return src.ToString(mask);
        }

        public static IEnumerable<AssetPair> WhichConsistsOfAssets(this IEnumerable<AssetPair> src,
            params string[] assetIds)
        {
            return src.Where(assetPair =>
                assetIds.Contains(assetPair.BaseAssetId) && assetIds.Contains(assetPair.QuotingAssetId));
        }

        public static IEnumerable<AssetPair> WhichHaveAssets(this IEnumerable<AssetPair> src, params string[] assetIds)
        {
            return src.Where(assetPair =>
                assetIds.Contains(assetPair.BaseAssetId) || assetIds.Contains(assetPair.QuotingAssetId));
        }
    }
}