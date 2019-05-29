using Lykke.Service.Assets.Core.Domain;
using ProtoBuf;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    [ProtoContract]
    public class AssetDefaultConditionDto : IAssetDefaultCondition
    {
        [ProtoMember(1)]
        public bool? AvailableToClient { get; set; }
        [ProtoMember(2)]
        public bool? IsTradable { get; set; }
        [ProtoMember(3)]
        public bool? BankCardsDepositEnabled { get; set; }
        [ProtoMember(4)]
        public bool? SwiftDepositEnabled { get; set; }
        [ProtoMember(5)]
        public string Regulation { get; set; }
    }
}
