using System;
using JetBrains.Annotations;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    [UsedImplicitly]
    public class DateTimeProvider : IDateTimeProvider
    {
        public DateTime UtcNow => DateTime.UtcNow;
    }
}