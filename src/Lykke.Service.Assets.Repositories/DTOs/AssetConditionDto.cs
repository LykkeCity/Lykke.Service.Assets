using Lykke.Service.Assets.Core.Domain;
using ProtoBuf;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    [ProtoContract]
    public class AssetConditionDto : IAssetCondition
    {
        [ProtoMember(1)]
        public string Asset { get; set; }
        [ProtoMember(2)]
        public string Regulation { get; set; }
        [ProtoMember(3)]
        public bool? AvailableToClient { get; set; }
        [ProtoMember(4)]
        public bool? IsTradable { get; set; }
        [ProtoMember(5)]
        public bool? BankCardsDepositEnabled { get; set; }
        [ProtoMember(6)]
        public bool? SwiftDepositEnabled { get; set; }
    }
}
