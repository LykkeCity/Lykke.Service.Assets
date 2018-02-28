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
            public DictionariesSettings Dictionaries { get; set; }
            public LogsSettings Logs { get; set; }
            public DbSettings Db { get; set; }
            public RabbitSettings Rabbit { get; set; }
            public RadisSettings RadisSettings { get; set; }
            public string QueuePostfix { get; set; }
            public TimeSpan RetryDelay { get; set; }
            public string SagasRabbitMqConnStr { get; set; }
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
        }

        public class DictionariesSettings
        {
            public string DbConnectionString { get; set; }
            public TimeSpan CacheExpirationPeriod { get; set; }
        }

        public class LogsSettings
        {
            public string DbConnectionString { get; set; }
        }

        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; }

            public int ThrottlingLimitSeconds { get; set; }
        }

        public class AzureQueueSettings
        {
            public string ConnectionString { get; set; }

            public string QueueName { get; set; }
        }

        public class RadisSettings : IAssetsForClientCacheManagerSettings
        {
            public string RedisConfiguration { get; set; }
            public string InstanceName { get; set; }
            public TimeSpan AssetsForClientCacheTimeSpan { get; set; }
        }
    }
}
