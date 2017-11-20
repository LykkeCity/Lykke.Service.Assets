using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;


namespace Lykke.Service.Assets.Services
{
    public class AssetGroupService : IAssetGroupService
    {
        private readonly IAssetGroupAssetLinkRepository  _assetGroupAssetLinkRepository;
        private readonly IClientAssetGroupLinkRepository _clientAssetGroupLinkRepository;
        private readonly IAssetGroupClientLinkRepository _assetGroupClientLinkRepository;
        private readonly IAssetGroupRepository           _assetGroupRepository;


        public AssetGroupService(
            IClientAssetGroupLinkRepository clientAssetGroupLinkRepository,
            IAssetGroupClientLinkRepository assetGroupClientLinkRepository,
            IAssetGroupAssetLinkRepository  assetGroupAssetLinkRepository,
            IAssetGroupRepository           assetGroupRepository)
        {
            _assetGroupRepository           = assetGroupRepository;
            _clientAssetGroupLinkRepository = clientAssetGroupLinkRepository;
            _assetGroupClientLinkRepository = assetGroupClientLinkRepository;
            _assetGroupAssetLinkRepository  = assetGroupAssetLinkRepository;
        }


        public async Task AddAssetToGroupAsync(string assetId, string groupName)
        {
            var assetGroup = await _assetGroupRepository.GetAsync(groupName);

            var assetGroupAssetLink = new AssetGroupAssetLink
            {
                AssetId                      = assetId,
                ClientsCanCashInViaBankCards = assetGroup.ClientsCanCashInViaBankCards,
                GroupName                    = assetGroup.Name,
                IsIosDevice                  = assetGroup.IsIosDevice,
                SwiftDepositEnabled          = assetGroup.SwiftDepositEnabled
            };

            await _assetGroupAssetLinkRepository.AddAsync(assetGroupAssetLink);
        }

        // TODO: Obsolete
        public async Task AddClientToGroupAsync(string clientId, string groupName)
        {
            await AddClientToGroupAsync(clientId, groupName, false);
        }

        public async Task AddClientToGroupOrReplaceAsync(string clientId, string groupName)
        {
            await AddClientToGroupAsync(clientId, groupName, true);
        }

        public async Task<IAssetGroup> AddGroupAsync(IAssetGroup group)
        {
            await _assetGroupRepository.AddAsync(group);

            return group;
        }
        
        public async Task<bool> CashInViaBankCardEnabledAsync(string clientId, bool isIosDevice)
        {
            var assetGroups = (await _assetGroupClientLinkRepository.GetAllAsync(clientId)).ToArray();

            var cashInViaBankCardsEnabledForDeviceInAnyGroup = assetGroups.Any(x => x.ClientsCanCashInViaBankCards && x.IsIosDevice == isIosDevice);
            var clientDeviceNotAssignedToAnyGroup            = assetGroups.All(x => x.IsIosDevice != isIosDevice);

            return cashInViaBankCardsEnabledForDeviceInAnyGroup
                || clientDeviceNotAssignedToAnyGroup;
        }

        public async Task<IEnumerable<IAssetGroup>> GetAllGroupsAsync()
        {
            return await _assetGroupRepository.GetAllAsync();
        }

        public async Task<IEnumerable<string>> GetAssetIdsForClient(string clientId, bool isIosDevice)
        {
            var clientAssetIds    = new List<string>();
            var clientAssetGroups = (await _assetGroupClientLinkRepository.GetAllAsync(clientId)).Where(x => x.IsIosDevice == isIosDevice);
            
            foreach (var group in clientAssetGroups)
            {
                var groupAssetIds = (await _assetGroupAssetLinkRepository.GetAllAsync(group.GroupName)).Select(x => x.AssetId);

                clientAssetIds.AddRange(groupAssetIds);
            }

            return clientAssetIds;
        }

        public async Task<IEnumerable<string>> GetAssetIdsForGroupAsync(string groupName)
        {
            return (await _assetGroupAssetLinkRepository.GetAllAsync(groupName))
                .Select(x => x.AssetId);
        }

        public async Task<IEnumerable<string>> GetClientIdsForGroupAsync(string groupName)
        {
            return (await _clientAssetGroupLinkRepository.GetAllAsync(groupName))
                .Select(x => x.ClientId);
        }

        public async Task<IAssetGroup> GetGroupAsync(string groupName)
        {
            return await _assetGroupRepository.GetAsync(groupName);
        }

        public async Task RemoveAssetFromGroupAsync(string assetId, string groupName)
        {
            await _assetGroupAssetLinkRepository.RemoveAsync(assetId, groupName);
        }

        public async Task RemoveClientFromGroupAsync(string clientId, string groupName)
        {
            await _clientAssetGroupLinkRepository.RemoveAsync(clientId, groupName);
            await _assetGroupClientLinkRepository.RemoveAsync(clientId, groupName);
        }

        public async Task RemoveGroupAsync(string groupName)
        {
            foreach (var assetId in await GetAssetIdsForGroupAsync(groupName))
            {
                await _assetGroupAssetLinkRepository.RemoveAsync(assetId, groupName);
            }

            foreach (var clientId in await GetClientIdsForGroupAsync(groupName))
            {
                await _clientAssetGroupLinkRepository.RemoveAsync(clientId, groupName);
                await _assetGroupClientLinkRepository.RemoveAsync(clientId, groupName);
            }

            await _assetGroupRepository.RemoveAsync(groupName);
        }

        public async Task<bool> SwiftDepositEnabledAsync(string clientId, bool isIosDevice)
        {
            var assetGroups = (await _assetGroupClientLinkRepository.GetAllAsync(clientId)).ToArray();

            var swiftDepositEnabledForDeviceInAnyGroup = assetGroups.Any(x => x.SwiftDepositEnabled && x.IsIosDevice == isIosDevice);
            var clientDeviceNotAssignedToAnyGroup      = assetGroups.All(x => x.IsIosDevice != isIosDevice);

            return swiftDepositEnabledForDeviceInAnyGroup
                || clientDeviceNotAssignedToAnyGroup;
        }

        public async Task UpdateGroupAsync(IAssetGroup group)
        {
            foreach (var assetId in await GetAssetIdsForGroupAsync(group.Name))
            {
                await _assetGroupAssetLinkRepository.UpdateAsync(assetId, group);
            }

            foreach (var clientId in await GetClientIdsForGroupAsync(group.Name))
            {
                await _clientAssetGroupLinkRepository.UpdateAsync(clientId, group);
                await _assetGroupClientLinkRepository.UpdateAsync(clientId, group);
            }

            await _assetGroupRepository.UpdateAsync(group);
        }

        private async Task AddClientToGroupAsync(string clientId, string groupName, bool replace)
        {
            var assetGroup = await _assetGroupRepository.GetAsync(groupName);

            var assetClientGroupLink = new ClientAssetGroupLink
            {
                ClientId = clientId,
                ClientsCanCashInViaBankCards = assetGroup.ClientsCanCashInViaBankCards,
                GroupName = assetGroup.Name,
                IsIosDevice = assetGroup.IsIosDevice,
                SwiftDepositEnabled = assetGroup.SwiftDepositEnabled
            };

            var assetGroupClientLink = new AssetGroupClientLink
            {
                ClientId = clientId,
                ClientsCanCashInViaBankCards = assetGroup.ClientsCanCashInViaBankCards,
                GroupName = assetGroup.Name,
                IsIosDevice = assetGroup.IsIosDevice,
                SwiftDepositEnabled = assetGroup.SwiftDepositEnabled
            };

            if (replace)
            {
                await _clientAssetGroupLinkRepository.AddOrReplaceAsync(assetClientGroupLink);
                await _assetGroupClientLinkRepository.AddOrReplaceAsync(assetGroupClientLink);
            }
            else
            {
                await _clientAssetGroupLinkRepository.AddAsync(assetClientGroupLink);
                await _assetGroupClientLinkRepository.AddAsync(assetGroupClientLink);
            }
        }
    }
}
