using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetGroupService
    {
        Task AddAssetToGroupAsync(string assetId, string groupName);

        Task AddClientToGroupAsync(string clientId, IAssetGroup assetGroup);

        Task AddClientToGroupOrReplaceAsync(string clientId, IAssetGroup assetGroup);

        Task<IAssetGroup> AddGroupAsync(IAssetGroup group);

        Task<bool> CashInViaBankCardEnabledAsync(string clientId, bool isIosDevice);

        Task<IEnumerable<IAssetGroup>> GetAllGroupsAsync();

        Task<IEnumerable<string>> GetAssetIdsForClient(string clientId, bool isIosDevice);

        Task<IEnumerable<string>> GetAssetIdsForGroupAsync(string groupName);

        Task<IEnumerable<string>> GetClientIdsForGroupAsync(string groupName);

        Task<IAssetGroup> GetGroupAsync(string groupName);

        Task RemoveAssetFromGroupAsync(string assetId, string groupName);

        Task RemoveClientFromGroupAsync(string clientId, string groupName);

        Task RemoveGroupAsync(string groupName);

        Task<bool> SwiftDepositEnabledAsync(string clientId, bool isIosDevice);

        Task UpdateGroupAsync(IAssetGroup group);
    }
}
