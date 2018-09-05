using Lykke.Common.Chaos;
using Lykke.SettingsReader.Attributes;
using System;

namespace Lykke.Service.Assets.Settings
{
    public class CqrsSettings
    {
        [AmqpCheck]
        public string RabbitConnectionString { get; set; }
        [Optional]
        public ChaosSettings ChaosKitty { get; set; }
        public string QueuePostfix { get; set; }
        public TimeSpan RetryDelay { get; set; }
    }
}
