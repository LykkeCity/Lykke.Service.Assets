using System;
using Lykke.Service.Assets.Client.Custom;

namespace Lykke.Service.Assets.Client.Models
{
    public partial class AssetDescriptionsResponseModel : Lykke.Service.Assets.Client.Custom.IAssetDescription
    {
        //public string Id => throw new NotImplementedException();

        public ErrorResponse errorResponse { get; set; }

        public static AssetDescriptionsResponseModel Create(ErrorResponse error)
        {
            return new AssetDescriptionsResponseModel
            {
                errorResponse = error
            };

        }
    }
}
