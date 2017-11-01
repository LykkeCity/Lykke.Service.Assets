using System.Threading.Tasks;
using Common.Log;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class ShutdownManager : IShutdownManager
    {
        private readonly ILog _log;

        public ShutdownManager(ILog log)
        {
            _log = log;
        }

        public async Task StopAsync()
        {
            await Task.CompletedTask;
        }
    }
}
