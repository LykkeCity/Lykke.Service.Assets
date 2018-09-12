using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Services
{
    public class AssetGroupService : IAssetGroupService
    {
        private readonly IAssetGroupAssetLinkRepository _assetGroupAssetLinkRepository;
        private readonly IClientAssetGroupLinkRepository _clientAssetGroupLinkRepository;
        private readonly IAssetGroupClientLinkRepository _assetGroupClientLinkRepository;
        private readonly IAssetGroupRepository _assetGroupRepository;
        private readonly IAssetsForClientCacheManager _cacheManager;
        private readonly IAssetConditionService _assetConditionService;


        public AssetGroupService(
            IClientAssetGroupLinkRepository clientAssetGroupLinkRepository,
            IAssetGroupClientLinkRepository assetGroupClientLinkRepository,
            IAssetGroupAssetLinkRepository assetGroupAssetLinkRepository,
            IAssetsForClientCacheManager cacheManager,
            IAssetGroupRepository assetGroupRepository,
            IAssetConditionService assetConditionService)
        {
            _assetGroupRepository = assetGroupRepository;
            _cacheManager = cacheManager;
            _assetConditionService = assetConditionService;
            _clientAssetGroupLinkRepository = clientAssetGroupLinkRepository;
            _assetGroupClientLinkRepository = assetGroupClientLinkRepository;
            _assetGroupAssetLinkRepository = assetGroupAssetLinkRepository;
        }


        public async Task AddAssetToGroupAsync(string assetId, string groupName)
        {
            var assetGroup = await _assetGroupRepository.GetAsync(groupName);

            var assetGroupAssetLink = new AssetGroupAssetLink
            {
                AssetId = assetId,
                ClientsCanCashInViaBankCards = assetGroup.ClientsCanCashInViaBankCards,
                GroupName = assetGroup.Name,
                IsIosDevice = assetGroup.IsIosDevice,
                SwiftDepositEnabled = assetGroup.SwiftDepositEnabled
            };

            await _assetGroupAssetLinkRepository.AddAsync(assetGroupAssetLink);
            await _cacheManager.ClearCacheAsync($"AddAssetToGroupAsync {groupName}, {assetId}");
        }

        // TODO: Obsolete
        public async Task AddClientToGroupAsync(string clientId, IAssetGroup assetGroup)
        {
            await AddClientToGroupAsync(clientId, assetGroup, false);
        }

        public async Task AddClientToGroupOrReplaceAsync(string clientId, IAssetGroup assetGroup)
        {
            await AddClientToGroupAsync(clientId, assetGroup, true);
        }

        public async Task<IAssetGroup> AddGroupAsync(IAssetGroup group)
        {
            await _assetGroupRepository.AddAsync(group);

            return group;
        }

        public async Task<bool> CashInViaBankCardEnabledAsync(string clientId, bool isIosDevice)
        {
            var cache = await _cacheManager.TryGetCashInViaBankCardEnabledForClientAsync(clientId, isIosDevice);
            if (cache != null)
            {
                return cache.Value;
            }

            var assetGroups = (await _assetGroupClientLinkRepository.GetAllAsync(clientId)).ToArray();

            var cashInViaBankCardsEnableForDeviceInAnyGroup = assetGroups.Any(x => x.ClientsCanCashInViaBankCards && x.IsIosDevice == isIosDevice);
            var clientDeviceNotAssignedToAnyGroup = assetGroups.All(x => x.IsIosDevice != isIosDevice);

            var conditions = await _assetConditionService.GetAssetConditionsLayerSettingsByClient(clientId);
            var conditionLayerCashInViaBankCardEnabled = conditions.ClientsCanCashInViaBankCards ?? true;

            var result =
                conditionLayerCashInViaBankCardEnabled &&
                (cashInViaBankCardsEnableForDeviceInAnyGroup || clientDeviceNotAssignedToAnyGroup);

            await _cacheManager.SaveCashInViaBankCardEnabledForClientAsync(clientId, isIosDevice, result);

            return result;
        }

        public async Task<IEnumerable<IAssetGroup>> GetAllGroupsAsync()
        {
            return await _assetGroupRepository.GetAllAsync();
        }

        public async Task<IEnumerable<string>> GetAssetIdsForClient(string clientId, bool isIosDevice)
        {
            var clientAssetGroups =
                (await _assetGroupClientLinkRepository.GetAllAsync(clientId)).Where(x =>
                    x.IsIosDevice == isIosDevice);

            var tasks = clientAssetGroups.Select(group => _assetGroupAssetLinkRepository.GetAllAsync(group.GroupName));
            var groupAssets = await Task.WhenAll(tasks.ToArray());
            return groupAssets.SelectMany(x => x).Select(x => x.AssetId);
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
            await _cacheManager.ClearCacheAsync($"RemoveAssetFromGroupAsync {groupName}, {assetId}");
        }

        public async Task RemoveClientFromGroupAsync(string clientId, string groupName)
        {
            await _clientAssetGroupLinkRepository.RemoveAsync(clientId, groupName);
            await _assetGroupClientLinkRepository.RemoveAsync(clientId, groupName);
            await _cacheManager.RemoveClientFromCacheAsync(clientId);

        }

        public async Task RemoveGroupAsync(string groupName)
        {
            var tasks = new List<Task>();
            var assetLinksTasks = (await GetAssetIdsForGroupAsync(groupName)).Select(assetId =>
                _assetGroupAssetLinkRepository.RemoveAsync(assetId, groupName));
            tasks.AddRange(assetLinksTasks);

            var clientIds = (await GetClientIdsForGroupAsync(groupName)).ToArray();
            var clientAssetGroupTasks =
                clientIds.Select(clientId => _clientAssetGroupLinkRepository.RemoveAsync(clientId, groupName));
            tasks.AddRange(clientAssetGroupTasks);
            var assetGroupTasks =
                clientIds.Select(clientId => _assetGroupClientLinkRepository.RemoveAsync(clientId, groupName));
            tasks.AddRange(assetGroupTasks);

            tasks.Add(_assetGroupRepository.RemoveAsync(groupName));
            await Task.WhenAll(tasks.ToArray());

            await _cacheManager.ClearCacheAsync($"RemoveGroupAsync {groupName}");
        }

        public async Task<bool> SwiftDepositEnabledAsync(string clientId, bool isIosDevice)
        {
            var cache = await _cacheManager.TryGetSwiftDepositEnabledForClientAsync(clientId, isIosDevice);
            if (cache != null)
            {
                return cache.Value;
            }

            var assetGroups = (await _assetGroupClientLinkRepository.GetAllAsync(clientId)).ToArray();

            var swiftDepositEnabledForDeviceInAnyGroup = assetGroups.Any(x => x.SwiftDepositEnabled && x.IsIosDevice == isIosDevice);
            var clientDeviceNotAssignedToAnyGroup = assetGroups.All(x => x.IsIosDevice != isIosDevice);

            var conditions = await _assetConditionService.GetAssetConditionsLayerSettingsByClient(clientId);
            var conditionLayerSwiftDepositEnabled = conditions.SwiftDepositEnabled ?? true;

            var result =
                conditionLayerSwiftDepositEnabled &&
                (swiftDepositEnabledForDeviceInAnyGroup || clientDeviceNotAssignedToAnyGroup);

            await _cacheManager.SaveSwiftDepositEnabledForClientAsync(clientId, isIosDevice, result);

            return result;
        }

        public async Task UpdateGroupAsync(IAssetGroup group)
        {
            var tasks = new List<Task>();
            var assetLinksTasks = (await GetAssetIdsForGroupAsync(group.Name)).Select(assetId =>
                _assetGroupAssetLinkRepository.UpdateAsync(assetId, group));
            tasks.AddRange(assetLinksTasks);

            var clientIds = (await GetClientIdsForGroupAsync(group.Name)).ToArray();
            var clientAssetGroupTasks =
                clientIds.Select(clientId => _clientAssetGroupLinkRepository.UpdateAsync(clientId, group));
            tasks.AddRange(clientAssetGroupTasks);
            var assetGroupTasks =
                clientIds.Select(clientId => _assetGroupClientLinkRepository.UpdateAsync(clientId, group));
            tasks.AddRange(assetGroupTasks);

            tasks.Add(_assetGroupRepository.UpdateAsync(group));
            await Task.WhenAll(tasks.ToArray());

            await _cacheManager.ClearCacheAsync($"UpdateGroupAsync {group.Name}");
        }

        private async Task AddClientToGroupAsync(string clientId, IAssetGroup assetGroup, bool replace)
        {
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

            await _cacheManager.RemoveClientFromCacheAsync(clientId);
        }
    }
}
