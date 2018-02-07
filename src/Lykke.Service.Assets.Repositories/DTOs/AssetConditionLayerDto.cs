using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetConditionLayerDto : IAssetConditionLayer
    {
        public AssetConditionLayerDto()
        {
            AssetConditions = new List<IAssetCondition>();
        }

        public string Id { get; set; }
        public IReadOnlyList<IAssetCondition> AssetConditions { get; set; }
        public IAssetDefaultCondition AssetDefaultCondition { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
    }
}
