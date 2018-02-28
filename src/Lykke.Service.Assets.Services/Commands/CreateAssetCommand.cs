using Lykke.Service.Assets.Services.Domain;
using MessagePack;

namespace Lykke.Service.Assets.Services.Commands
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class CreateAssetCommand
    {
        public Asset Asset { get; set; }
    }
}
