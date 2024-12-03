using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Assets.Settings
{
    public class MyNoSqlSettings
    {
        public string WriterServiceUrl { get; set; }

        [Optional]
        public int MaxClientsInCache { get; set; } = 10000;
    }
}
