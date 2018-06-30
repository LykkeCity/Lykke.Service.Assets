using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Logs;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    [UsedImplicitly]
    public class StartupManager : IStartupManager
    {
        private readonly ILog _log;

        public StartupManager(ILogFactory logFactory)
        {
            _log = logFactory.CreateLog(this);
        }

        public async Task StartAsync()
        {
            await Task.CompletedTask;
        }
    }
}
