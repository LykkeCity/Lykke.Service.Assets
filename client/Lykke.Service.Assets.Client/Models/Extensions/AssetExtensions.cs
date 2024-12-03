using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.Models.Extensions
{
    /// <summary>
    /// 
    /// </summary>
    public static class AssetExtensions
    {
        public static double Multiplier(this Asset asset)
        {
            return Math.Pow(10, asset.MultiplierPower * -1);
        }

        public static string GetFirstAssetId(this IEnumerable<Asset> assets)
        {
            return assets.OrderBy(x => x.DefaultOrder).First().Id;
        }
    }
}
