using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetAttribute : IAssetAttribute
    {
        public string Key { get; set; }

        public string Value { get; set; }
    }
}