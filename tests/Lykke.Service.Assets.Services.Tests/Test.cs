using AutoMapper;

namespace Lykke.Service.Assets.Services.Tests
{
    public abstract class Test
    {
        private static readonly object Sync = new object();
        private static readonly bool Configured;

        static Test()
        {
            lock (Sync)
            {
                if (!Configured)
                {
                    Mapper.Initialize(m => m.AddProfile<AutoMapperProfile>());
                    Mapper.AssertConfigurationIsValid();
                }
                Configured = true;
            }
        }
    }
}
