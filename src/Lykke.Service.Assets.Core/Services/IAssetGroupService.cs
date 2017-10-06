using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetGroupService
    {
        Task<bool> CashInViaBankCardEnabled(string clientId, bool isIosDevice);

        Task<bool> SwiftDepositEnabled(string clientId, bool isIosDevice);
    }
}