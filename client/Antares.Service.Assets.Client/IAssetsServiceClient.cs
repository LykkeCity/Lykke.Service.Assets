using System.Collections.Generic;
using System.Threading.Tasks;
using Antares.Service.Assets.Client.Models;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.Assets.Core.Domain;

namespace Antares.Service.Assets.Client
{
    public interface IAssetsServiceClient
    {
        IAssetAttributesClient AssetAttributes { get; }

        IAssetCategoryClient AssetCategory { get; }

        IAssetExtendedInfoClient AssetExtendedInfo { get; }

        IAssetsClient Assets { get; }

        IAssetPairsClient AssetPairs { get; }

        IAssetsServiceHttp HttpClient { get; }
    }


    public interface IAssetAttributesClient
    {
        IAssetAttribute Get(string assetId, string key);

        IList<IAssetAttributes> GetAll();

        IAssetAttributes GetAllForAsset(string assetId);
    }

    public interface IAssetCategoryClient
    {
        IAssetCategory Get(string id);
        IList<IAssetCategory> GetAll();
    }

    public interface IAssetExtendedInfoClient
    {
        IAssetExtendedInfo Get(string id);
        List<IAssetExtendedInfo> GetAll();
        IAssetExtendedInfo GetDefault();
    }

    public interface IAssetsClient
    {
        IAsset Get(string assetId);
        List<IAsset> GetAll(bool includeNonTradable = false);

        List<IAsset> GetBySpecification(IReadOnlyList<string> ids = null, bool? IsTradable = null);
    }

    public interface IAssetPairsClient
    {
        IAssetPair Get(string id);
        List<IAssetPair> GetAll();
    }






}
