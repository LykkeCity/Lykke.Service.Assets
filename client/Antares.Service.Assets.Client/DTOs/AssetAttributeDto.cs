using Lykke.Service.Assets.Core.Domain;

namespace Antares.Service.Assets.Client.DTOs
{
    internal class AssetAttributeDto : IAssetAttribute
    {
        public string Key { get; set; }
        public string Value { get; set; }
    }
}
