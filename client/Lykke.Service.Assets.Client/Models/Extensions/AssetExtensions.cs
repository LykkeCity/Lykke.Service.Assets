using System;
using System.Collections.Generic;
using System.Linq;

namespace Lykke.Service.Assets.Client.Models.Extensions
{
    public static class AssetExtensions
    {
        public static int Multiplier(this Asset asset)
        {
            return (int) Math.Pow(10, asset.MultiplierPower * -1);
        }

        public static string GetFirstAssetId(this IEnumerable<Asset> assets)
        {
            return assets.OrderBy(x => x.DefaultOrder).First().Id;
        }
    }
}