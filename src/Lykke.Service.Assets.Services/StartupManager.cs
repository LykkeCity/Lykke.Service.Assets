using System.Threading.Tasks;
using Antares.Sdk.Services;
using Lykke.Cqrs;

namespace Lykke.Service.Assets.Services
{
    public class StartupManager : IStartupManager
    {
        private readonly ICqrsEngine _cqrsEngine;

        public StartupManager(ICqrsEngine cqrsEngine)
        {
            _cqrsEngine = cqrsEngine;
        }

        public Task StartAsync()
        {
            _cqrsEngine.StartSubscribers();

            return Task.CompletedTask;
        }
    }
}
