using System.Collections.Generic;
using Lykke.Service.Assets.Attributes;

namespace Lykke.Service.Assets.Responses.v2.AssetConditions
{
    public class AssetConditionLayerModel
    {
        public AssetConditionLayerModel()
        {
            AssetConditions = new List<AssetConditionModel>();
        }

        [KeyFormat]
        public string Id { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public List<AssetConditionModel> AssetConditions { get; set; }
        public AssetDefaultConditionModel DefaultCondition { get; set; }
    }
}
