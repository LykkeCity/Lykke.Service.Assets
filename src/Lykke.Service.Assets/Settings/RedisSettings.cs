using System;
using Lykke.SettingsReader.Attributes;

namespace Lykke.Service.Assets.Settings
{
    public partial class ApplicationSettings
    {
        public class RedisSettings
        {
            public string Configuration { get; set; }
            public string Instance { get; set; }
            [Optional]
            public TimeSpan Expiration { get; set; } = TimeSpan.FromDays(1);
        }
    }
}
