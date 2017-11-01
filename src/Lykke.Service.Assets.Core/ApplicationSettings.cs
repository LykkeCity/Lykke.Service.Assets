﻿using System;
using JetBrains.Annotations;

namespace Lykke.Service.Assets.Core
{
    [UsedImplicitly]
    public class ApplicationSettings
    {
        public AssetsSettings AssetsService { get; set; }
        
        public SlackNotificationsSettings SlackNotifications { get; set; }

        [UsedImplicitly]
        public class AssetsSettings
        {
            public DictionariesSettings Dictionaries { get; set; }
            public LogsSettings Logs { get; set; }
            public DbSettings Db { get; set; }
            public RabbitSettings Rabbit { get; set; }
        }

        public class RabbitSettings
        {
            public string ConnectionString { get; set; }
        }

        [UsedImplicitly]
        public class DbSettings
        {
            public string ClientPersonalInfoConnString { get; set; }
        }

        [UsedImplicitly]
        public class DictionariesSettings
        {
            public string DbConnectionString { get; set; }
            public TimeSpan CacheExpirationPeriod { get; set; }
        }

        [UsedImplicitly]
        public class LogsSettings
        {
            public string DbConnectionString { get; set; }
        }

        [UsedImplicitly]
        public class SlackNotificationsSettings
        {
            public AzureQueueSettings AzureQueue { get; set; }

            public int ThrottlingLimitSeconds { get; set; }
        }

        [UsedImplicitly]
        public class AzureQueueSettings
        {
            public string ConnectionString { get; set; }

            public string QueueName { get; set; }
        }
    }

    
}