using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Assets.Client
{
    public class AssetServiceSettings
    {
        [HttpCheck("/api/isalive")]
        public string ServiceUrl { get; set; }
    }
}
