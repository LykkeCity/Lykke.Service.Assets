using System;

namespace Lykke.Service.Assets.Client.Custom
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}