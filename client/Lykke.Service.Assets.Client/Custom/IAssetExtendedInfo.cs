using Lykke.Service.Assets.Client.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Client.Custom
{
    public interface IAssetDescription
    {
        IList<AssetDescription> Descriptions { get; set; }

        ErrorResponse ErrorResponse { get; set; }
    }
}
