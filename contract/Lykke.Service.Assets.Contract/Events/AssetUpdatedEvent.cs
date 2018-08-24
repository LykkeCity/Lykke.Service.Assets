using MessagePack;
using ProtoBuf;

namespace Lykke.Service.Assets.Contract.Events
{
    [ProtoContract]
    public class AssetUpdatedEvent
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public string Name { get; set; }

        [ProtoMember(3)]
        public string DisplayId { get; set; }

        [ProtoMember(4)]
        public int Accuracy { get; set; }

        [ProtoMember(5)]
        public double? LowVolumeAmount { get; set; }

        [ProtoMember(6)]
        public double CashoutMinimalAmount { get; set; }

        [ProtoMember(7)]
        public string Symbol { get; set; }

        [ProtoMember(8)]
        public bool HideWithdraw { get; set; }

        [ProtoMember(9)]
        public bool HideDeposit { get; set; }

        [ProtoMember(10)]
        public bool KycNeeded { get; set; }

        [ProtoMember(11)]
        public bool BankCardsDepositEnabled { get; set; }

        [ProtoMember(12)]
        public bool SwiftDepositEnabled { get; set; }

        [ProtoMember(13)]
        public bool BlockchainDepositEnabled { get; set; }

        [ProtoMember(14)]
        public string CategoryId { get; set; }

        [ProtoMember(15)]
        public bool IsBase { get; set; }

        [ProtoMember(16)]
        public string IconUrl { get; set; }
    }
}
