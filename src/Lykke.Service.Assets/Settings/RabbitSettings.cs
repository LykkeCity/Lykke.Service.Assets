using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Assets.Settings
{
    public partial class ApplicationSettings
    {
        public class RabbitSettings
        {
            [AmqpCheck]
            public string ConnectionString { get; set; }
        }
    }
}
