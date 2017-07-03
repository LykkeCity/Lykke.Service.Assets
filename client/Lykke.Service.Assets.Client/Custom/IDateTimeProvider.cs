using System;

namespace Lykke.Service.Assets.Client.Custom
{
    public interface IDateTimeProvider
    {
        DateTime UtcNow { get; }
    }
}