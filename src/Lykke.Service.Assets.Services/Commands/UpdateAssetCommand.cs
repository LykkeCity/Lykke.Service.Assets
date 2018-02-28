using Lykke.Service.Assets.Services.Domain;
using MessagePack;

namespace Lykke.Service.Assets.Services.Commands
{
    [MessagePackObject(keyAsPropertyName: true)]
    public class UpdateAssetCommand
    {
        public Asset Asset { get; set; }
    }
}
