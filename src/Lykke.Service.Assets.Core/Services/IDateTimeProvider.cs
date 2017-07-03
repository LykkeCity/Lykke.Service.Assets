using System;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}