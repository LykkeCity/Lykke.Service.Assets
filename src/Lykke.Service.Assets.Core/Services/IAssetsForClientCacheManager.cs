using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetsForClientCacheManager
    {
        Task RemoveClientFromChache(string clientId);

        Task SaveAssetForClient(string clientId, bool isIosDevice, IEnumerable<string> clientAssetIds);
        Task SaveCashInViaBankCardEnabledForClient(string clientId, bool isIosDevice, bool cashInViaBankCardEnabled);
        Task SaveSwiftDepositEnabledForClient(string clientId, bool isIosDevice, bool swiftDepositEnabled);

        Task<IReadOnlyList<string>> TryGetAssetForClient(string clientId, bool isIosDevice);
        Task<bool?> TryGetSaveCashInViaBankCardEnabledForClient(string clientId, bool isIosDevice);
        Task<bool?> TryGetSaveSwiftDepositEnabledForClient(string clientId, bool isIosDevice);
    }
}
