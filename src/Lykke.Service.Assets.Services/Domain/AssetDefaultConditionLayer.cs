using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetDefaultConditionLayer : IAssetDefaultConditionLayer
    {
        public string Id { get; set; }
        public IList<IAssetCondition> AssetConditions { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
    }
}
