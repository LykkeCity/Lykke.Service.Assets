using System;
using Lykke.SettingsReader.Attributes;
using Lykke.Common.Chaos;

namespace Lykke.Service.Assets.Core
{
    public class ApplicationSettings
    {
        public AssetsSettings AssetsService { get; set; }

        public SlackNotificationsSettings SlackNotifications { get; set; }

        public class AssetsSettings
        {
            public DbSettings Db { get; set; }
            public RabbitSettings Rabbit { get; set; }
            public RedisSettings RedisSettings { get; set; }
            public CqrsSettings Cqrs { get; set; }
            [Optional]
            public ChaosSettings ChaosKitty { get; set; }
        }

        public class RabbitSettings
        {
            public string ConnectionString { get; set; }
        }

        public class DbSettings
        {
            public string ClientPersonalInfoConnString { get; set; }
            public string LogsConnString { get; set; }
            public string DictionariesConnectionString { get; set; }
        }

        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; }

        }

        public class AzureQueueSettings
        {
            public string ConnectionString { get; set; }

            public string QueueName { get; set; }
        }

        public class RedisSettings : IAssetsForClientCacheManagerSettings
        {
            public string Configuration { get; set; }
            public string Instance { get; set; }
            public TimeSpan Expiration { get; set; }
            public TimeSpan AssetsForClientCacheTimeSpan { get; set; }
        }

        public class CqrsSettings
        {
            public string RabbitConnectionString { get; set; }
        }
    }
}
