using System;
using Lykke.SettingsReader.Attributes;
using Lykke.Common.Chaos;
using Lykke.Sdk.Settings;

namespace Lykke.Service.Assets.Settings
{
    public partial class ApplicationSettings : BaseAppSettings
    {
        public AssetsSettings AssetsService { get; set; }
        
        public class AssetsSettings
        {
            public DbSettings Db { get; set; }
            public RabbitSettings Rabbit { get; set; }
            public RedisSettings RedisSettings { get; set; }
            public CqrsSettings Cqrs { get; set; }
        }

        public class RabbitSettings
        {
            public string ConnectionString { get; set; }
        }
    }
}
