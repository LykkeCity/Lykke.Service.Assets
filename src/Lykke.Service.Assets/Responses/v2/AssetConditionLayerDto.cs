using System.Collections.Generic;

namespace Lykke.Service.Assets.Responses.v2
{
    public class AssetConditionLayerDto
    {
        public AssetConditionLayerDto()
        {
            AssetConditions = new List<AssetConditionDto>();
        }

        public string Id { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public List<AssetConditionDto> AssetConditions { get; set; }
    }
}
