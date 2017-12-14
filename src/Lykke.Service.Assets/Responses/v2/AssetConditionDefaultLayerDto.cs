using System;
using System.Collections.Generic;
using System.Text;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.v2
{
    public class AssetConditionDefaultLayerDto : IAssetConditionDefaultLayer
    {
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public bool? AvailableToClient { get; set; }
        public string Regulation { get; set; }
    }
}
