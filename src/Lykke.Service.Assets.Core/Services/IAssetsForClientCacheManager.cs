using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetsForClientCacheManager
    {
        Task ClearCacheAsync(string reason);
        Task RemoveClientFromCacheAsync(string clientId);
        Task SaveAssetForClientAsync(string clientId, bool isIosDevice, IEnumerable<string> clientAssetIds);
        Task SaveCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice, bool cashInViaBankCardEnabled);
        Task SaveSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice, bool swiftDepositEnabled);
        Task SaveAssetConditionsForClientAsync(string clientId, IList<IAssetCondition> conditions);
        Task<IReadOnlyList<string>> TryGetAssetForClientAsync(string clientId, bool isIosDevice);
        Task<bool?> TryGetCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice);
        Task<bool?> TryGetSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice);
        Task<IList<IAssetCondition>> TryGetAssetConditionsForClientAsync(string clientId);
    }
}
