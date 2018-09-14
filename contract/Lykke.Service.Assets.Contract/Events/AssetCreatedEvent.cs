using MessagePack;
using ProtoBuf;

namespace Lykke.Service.Assets.Contract.Events
{
    [MessagePackObject(true)]
    [ProtoContract]
    public class AssetCreatedEvent
    {
        [ProtoMember(1, IsRequired = true)]
        public string Id { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public bool IsDisabled { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public string Name { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public string DisplayId { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public int Accuracy { get; set; }

        [ProtoMember(6)]
        public double? LowVolumeAmount { get; set; }

        [ProtoMember(7)]
        public double CashoutMinimalAmount { get; set; }
    }
}
