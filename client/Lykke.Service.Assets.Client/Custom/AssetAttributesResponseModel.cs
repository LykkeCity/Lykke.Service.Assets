using System.Collections.Generic;
using Lykke.Service.Assets.Client.Custom;

namespace Lykke.Service.Assets.Client.Models
{
    public partial class AssetAttributesResponseModel : IDictionaryItemModel, IAssetAttributes
    {
        public string Id => AssetId;

        public ErrorResponse errorResponse { get; set; }

        public static AssetAttributesResponseModel Create(ErrorResponse error)
        {
            return new AssetAttributesResponseModel
            {
                errorResponse = error
            };

        }
    }
}
