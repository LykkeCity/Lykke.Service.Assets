using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IShutdownManager
    {
        Task StopAsync();
    }
}
