using System.Threading.Tasks;

namespace Lykke.Job.Asset.Core.Services
{
    public interface IStartupManager
    {
        Task StartAsync();
    }
}