using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Assets.Settings
{
    public partial class ApplicationSettings
    {
        public class DbSettings
        {
            [AzureTableCheck]
            public string LogsConnString { get; set; }

            [AzureTableCheck]
            public string ClientPersonalInfoConnString { get; set; }

            [AzureTableCheck]
            public string DictionariesConnectionString { get; set; }
        }
    }
}
