using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetDefaultConditionLayerDto : IAssetDefaultConditionLayer
    {
        public AssetDefaultConditionLayerDto()
        {
            AssetConditions = new List<IAssetCondition>();
        }

        public string Id { get; set; }
        public IReadOnlyList<IAssetCondition> AssetConditions { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
    }
}
