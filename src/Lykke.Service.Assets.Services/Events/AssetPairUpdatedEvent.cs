using Lykke.Service.Assets.Services.Domain;
using MessagePack;

namespace Lykke.Service.Assets.Services.Events
{
    //TODO: move to contracts
    [MessagePackObject(keyAsPropertyName: true)]
    public class AssetPairUpdatedEvent
    {
        public AssetPair AssetPair { get; set; }
    }
}
