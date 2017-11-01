using System.Threading.Tasks;

namespace Lykke.Service.Assets.Managers
{
    public interface ICanInvalidateCache
    {
        Task InvalidateCache();
    }
}
