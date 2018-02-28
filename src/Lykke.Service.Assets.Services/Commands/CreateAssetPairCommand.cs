using Lykke.Service.Assets.Services.Domain;
using MessagePack;

namespace Lykke.Service.Assets.Services.Commands
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class CreateAssetPairCommand
    {
        public AssetPair AssetPair { get; set; }
    }
}
