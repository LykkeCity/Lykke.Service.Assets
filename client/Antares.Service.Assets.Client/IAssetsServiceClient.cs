using System.Collections.Generic;
using System.Threading.Tasks;
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
        IList<IAssetExtendedInfo> GetAll();
    }

    public interface IAssetsClient
    {
        IAsset Get(string id);
        IList<IAsset> GetAll(bool includeNonTradable = false);

        IList<IAsset> GetBySpecification(IReadOnlyList<string> ids = null, bool? IsTradable = null);
    }
}
