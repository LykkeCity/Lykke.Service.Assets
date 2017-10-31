using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class StartupManager : IStartupManager
    {
        private readonly ILog _log;

        public StartupManager(ILog log)
        {
            _log = log;
        }

        public async Task StartAsync()
        {
            await Task.CompletedTask;
        }
    }
}
