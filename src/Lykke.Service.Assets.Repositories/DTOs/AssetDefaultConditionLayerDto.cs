using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;
using ProtoBuf;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    [ProtoContract]
    public class AssetDefaultConditionLayerDto : IAssetDefaultConditionLayer
    {
        public AssetDefaultConditionLayerDto()
        {
            AssetConditions = new List<IAssetCondition>();
        }

        [ProtoMember(1)]
        public string Id { get; set; }
        public IReadOnlyList<IAssetCondition> AssetConditions { get; set; }
        [ProtoMember(2)]
        public bool? ClientsCanCashInViaBankCards { get; set; }
        [ProtoMember(3)]
        public bool? SwiftDepositEnabled { get; set; }
    }
}
