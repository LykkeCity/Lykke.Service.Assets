﻿using MessagePack;
using ProtoBuf;

namespace Lykke.Service.Assets.Client.Events
{
    [MessagePackObject(true)]
    [ProtoContract]
    public class AssetPairUpdatedEvent
    {
        [ProtoMember(1, IsRequired = true)]
        public string Id { get; set; }

        [ProtoMember(2, IsRequired = true)]
        public string Name { get; set; }

        [ProtoMember(3, IsRequired = true)]
        public string BaseAssetId { get; set; }

        [ProtoMember(4, IsRequired = true)]
        public string QuotingAssetId { get; set; }

        [ProtoMember(5, IsRequired = true)]
        public int Accuracy { get; set; }

        [ProtoMember(6, IsRequired = true)]
        public int InvertedAccuracy { get; set; }

        [ProtoMember(7, IsRequired = true)]
        public decimal MinVolume { get; set; }

        [ProtoMember(8, IsRequired = true)]
        public decimal MinInvertedVolume { get; set; }

        [ProtoMember(9, IsRequired = true)]
        public bool IsDisabled { get; set; }

        [ProtoMember(10, IsRequired = true)]
        public string ExchangeId { get; set; }
    }
}
