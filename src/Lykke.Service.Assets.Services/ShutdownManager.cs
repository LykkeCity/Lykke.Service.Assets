using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    [UsedImplicitly]
    public class ShutdownManager : IShutdownManager
    {
        private readonly ILog _log;

        public ShutdownManager(ILogFactory logFactory)
        {
            _log = logFactory.CreateLog(this);
        }

        public async Task StopAsync()
        {
            await Task.CompletedTask;
        }
    }
}
