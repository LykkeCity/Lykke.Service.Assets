using System.Threading.Tasks;

namespace Lykke.Job.Asset.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}