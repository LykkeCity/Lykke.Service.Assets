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
}
