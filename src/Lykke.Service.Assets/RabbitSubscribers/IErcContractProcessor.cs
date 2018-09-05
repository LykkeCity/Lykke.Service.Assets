using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.RabbitSubscribers
{
    public interface IErcContractProcessor
    {
        Task ProcessErc20ContractAsync(IErc20Token token);
    }
}
