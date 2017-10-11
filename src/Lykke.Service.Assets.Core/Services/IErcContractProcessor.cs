using Lykke.Service.Assets.Core.Domain;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IErcContractProcessor
    {
        Task ProcessErc20ContractAsync(IErc20Token message);
    }
}
