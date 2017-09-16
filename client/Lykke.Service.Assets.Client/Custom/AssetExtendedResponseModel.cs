using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Client.Models
{
    public partial class AssetExtendedResponseModel
    {
        public ErrorResponse errorResponse { get; set; }

        public static AssetExtendedResponseModel Create(ErrorResponse error)
        {
            return new AssetExtendedResponseModel
            {
                errorResponse = error
            };

        }
    }
}
