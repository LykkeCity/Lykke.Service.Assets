using System;

namespace Lykke.Service.Assets.Client.Cache
{
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}
