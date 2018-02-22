using Lykke.Service.Assets.Services.Domain;
using MessagePack;

namespace Lykke.Service.Assets.Services.Events
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class AssetCreatedEvent
    {
        public Asset Asset { get; set; }
    }
}
