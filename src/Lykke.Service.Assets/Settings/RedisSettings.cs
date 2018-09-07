using System;

namespace Lykke.Service.Assets.Settings
{
    public partial class ApplicationSettings
    {
        public class RedisSettings
        {
            public string Configuration { get; set; }
            public string Instance { get; set; }
            public TimeSpan Expiration { get; set; }
        }
    }
}
