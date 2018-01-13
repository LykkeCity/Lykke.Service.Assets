using System.Collections.Generic;

namespace Lykke.Service.Assets.Responses.v2.AssetConditions
{
    public class AssetDefaultConditionLayerModel
    {
        public AssetDefaultConditionLayerModel()
        {
            AssetConditions = new List<AssetConditionModel>();
        }

        public string Id { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public List<AssetConditionModel> AssetConditions { get; set; }
    }
}
