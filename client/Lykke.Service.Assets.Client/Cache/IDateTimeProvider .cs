using System;

namespace Lykke.Service.Assets.Client.Cache
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}
