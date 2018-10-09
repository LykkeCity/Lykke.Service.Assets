using Lykke.Service.Assets.Core.Domain;
using ProtoBuf;

namespace Lykke.Service.Assets.Responses.V2
{
    [ProtoContract]
    public class AssetPair : IAssetPair
    {
        [ProtoMember(1)]
        public string Id { get; set; }

        [ProtoMember(2)]
        public string BaseAssetId { get; set; }

        [ProtoMember(3)]
        public string QuotingAssetId { get; set; }

        [ProtoMember(4)]
        public int Accuracy { get; set; }

        [ProtoMember(5)]
        public int InvertedAccuracy { get; set; }

        [ProtoMember(6)]
        public bool IsDisabled { get; set; }

        [ProtoMember(7)]
        public string Name { get; set; }

        [ProtoMember(8)]
        public double MinVolume { get; set; }

        [ProtoMember(9)]
        public double MinInvertedVolume { get; set; }

        [ProtoMember(10)]
        public string Source { get; set; }

        [ProtoMember(11)]
        public string Source2 { get; set; }

        [ProtoMember(12)]
        public string ExchangeId { get; set; }
    }
}
