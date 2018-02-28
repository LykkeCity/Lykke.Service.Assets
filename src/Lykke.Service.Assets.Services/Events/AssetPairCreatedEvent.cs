﻿using Lykke.Service.Assets.Services.Domain;
using MessagePack;

namespace Lykke.Service.Assets.Services.Events
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class AssetPairCreatedEvent
    {
        public AssetPair AssetPair { get; set; }
    }
}
