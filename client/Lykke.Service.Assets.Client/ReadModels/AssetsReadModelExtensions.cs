using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Client.Models.v3;

namespace Lykke.Service.Assets.Client.ReadModels
{
    public static class AssetsReadModelExtensions
    {
        public static Asset GetIfEnabled(this IAssetsReadModel readModel, string id)
        {
            var asset = readModel.Get(id);
            return asset != null && !asset.IsDisabled ? asset : null;
        }

        public static AssetPair GetIfEnabled(this IAssetPairsReadModel readModel, string id)
        {
            var assetPair = readModel.Get(id);
            return assetPair != null && !assetPair.IsDisabled ? assetPair : null;
        }

        public static IReadOnlyCollection<Asset> GetAllEnabled(this IAssetsReadModel readModel)
        {
            var assets = readModel.GetAll();
            return assets.Where(x => !x.IsDisabled).ToArray();
        }

        public static IReadOnlyCollection<AssetPair> GetAllEnabled(this IAssetPairsReadModel readModel, string id)
        {
            var assetPairs = readModel.GetAll();
            return assetPairs.Where(x => !x.IsDisabled).ToArray();
        }
    }
}
