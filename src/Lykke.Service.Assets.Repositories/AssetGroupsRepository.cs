﻿using AzureStorage;
using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetGroupsRepository : IAssetGroupRepository, IDictionaryRepository<IAssetGroup>
    {
        readonly INoSQLTableStorage<AssetGroupEntity> _tableStorage;

        public AssetGroupsRepository(INoSQLTableStorage<AssetGroupEntity> tableStorage)
        {
            _tableStorage = tableStorage;
        }

        public Task RegisterGroup(string group, bool isIosDevice, bool clientsCanCashInViaBankCards, bool swiftDepositEnabled)
        {
            var entity = AssetGroupEntity.Record.Create(group, isIosDevice, clientsCanCashInViaBankCards, swiftDepositEnabled);
            return _tableStorage.InsertOrReplaceAsync(entity);
        }

        public async Task EditGroup(IAssetGroup assetGroup)
        {
            var entity = AssetGroupEntity.Record.Create(assetGroup);
            await _tableStorage.InsertOrMergeAsync(entity);

            var updatedGroup = await _tableStorage.GetDataAsync(AssetGroupEntity.Record.GeneratePartitionKey(),
                AssetGroupEntity.Record.GenerateRowKey(assetGroup.Name));

            var clients = (await GetClientIdsForGroup(assetGroup.Name)).ToArray();

            if (clients.Any())
            {
                foreach (var clientId in clients)
                {
                    var clientGroupLink = await _tableStorage
                        .GetDataAsync(AssetGroupEntity.ClientGroupLink.GeneratePartitionKey(updatedGroup.Name),
                            AssetGroupEntity.ClientGroupLink.GenerateRowKey(clientId));
                    AssetGroupEntity.ClientGroupLink.Update(clientGroupLink, updatedGroup);

                    var groupClientLink = await _tableStorage
                        .GetDataAsync(AssetGroupEntity.GroupClientLink.GeneratePartitionKey(clientId),
                            AssetGroupEntity.GroupClientLink.GenerateRowKey(updatedGroup.Name));
                    AssetGroupEntity.GroupClientLink.Update(groupClientLink, updatedGroup);

                    await _tableStorage.InsertOrMergeAsync(clientGroupLink);
                    await _tableStorage.InsertOrMergeAsync(groupClientLink);
                }
            }
        }

        public Task RemoveGroup(string group)
        {
            return _tableStorage.DeleteAsync(AssetGroupEntity.Record.GeneratePartitionKey(),
                AssetGroupEntity.Record.GenerateRowKey(group));
        }

        public async Task<IAssetGroup> GetAsync(string name)
        {
            return await _tableStorage.GetDataAsync(AssetGroupEntity.Record.GeneratePartitionKey(),
                AssetGroupEntity.Record.GenerateRowKey(name));
        }

        public async Task<IEnumerable<IAssetGroup>> GetAllGroups()
        {
            return await _tableStorage.GetDataAsync(AssetGroupEntity.Record.GeneratePartitionKey());
        }

        public async Task AddClientToGroup(string clientId, string group)
        {
            var groupRecord = await _tableStorage.GetDataAsync(AssetGroupEntity.Record.GeneratePartitionKey(),
                AssetGroupEntity.Record.GenerateRowKey(group));
            var cgEntity = AssetGroupEntity.ClientGroupLink.Create(group, clientId, groupRecord.IsIosDevice,
                groupRecord.ClientsCanCashInViaBankCards, groupRecord.SwiftDepositEnabled);
            var gcEntity = AssetGroupEntity.GroupClientLink.Create(group, clientId, groupRecord.IsIosDevice,
                groupRecord.ClientsCanCashInViaBankCards, groupRecord.SwiftDepositEnabled);
            await _tableStorage.InsertOrReplaceAsync(cgEntity);
            await _tableStorage.InsertOrReplaceAsync(gcEntity);
        }

        public async Task RemoveClientFromGroup(string clientId, string group)
        {
            await _tableStorage.DeleteAsync(AssetGroupEntity.ClientGroupLink.GeneratePartitionKey(group),
                AssetGroupEntity.ClientGroupLink.GenerateRowKey(clientId));
            await _tableStorage.DeleteAsync(AssetGroupEntity.GroupClientLink.GeneratePartitionKey(clientId),
                AssetGroupEntity.GroupClientLink.GenerateRowKey(group));
        }

        public async Task<IEnumerable<string>> GetClientIdsForGroup(string group)
        {
            return (await _tableStorage.GetDataAsync(AssetGroupEntity.ClientGroupLink.GeneratePartitionKey(group))).Select(x => x.RowKey);
        }

        public async Task AddAssetToGroup(string assetId, string group)
        {
            var groupRecord = await _tableStorage.GetDataAsync(AssetGroupEntity.Record.GeneratePartitionKey(),
                AssetGroupEntity.Record.GenerateRowKey(group));
            var entity = AssetGroupEntity.AssetLink.Create(group, assetId, groupRecord.IsIosDevice,
                groupRecord.ClientsCanCashInViaBankCards, groupRecord.SwiftDepositEnabled);
            await _tableStorage.InsertOrReplaceAsync(entity);
        }

        public Task RemoveAssetFromGroup(string assetId, string group)
        {
            return _tableStorage.DeleteAsync(AssetGroupEntity.AssetLink.GeneratePartitionKey(group),
                AssetGroupEntity.AssetLink.GenerateRowKey(assetId));
        }

        public async Task<IEnumerable<string>> GetAssetIdsForGroup(string group)
        {
            return (await _tableStorage.GetDataAsync(AssetGroupEntity.AssetLink.GeneratePartitionKey(group))).Select(x => x.RowKey);
        }

        public async Task<IEnumerable<string>> GetAssetIdsForClient(string clientId, bool isIosDevice)
        {
            var groups =
                (await _tableStorage.GetDataAsync(AssetGroupEntity.GroupClientLink.GeneratePartitionKey(clientId)))
                    .Where(x => x.IsIosDevice == isIosDevice).ToArray();

            if (groups.Any())
            {
                var result = new List<string>();

                foreach (var group in groups)
                {
                    result.AddRange((await _tableStorage.GetDataAsync(AssetGroupEntity.AssetLink.GeneratePartitionKey(group.Name)))
                        .Select(x => x.AssetId));
                }

                return result;
            }

            return null;
        }

        public async Task<bool> CanClientCashInViaBankCard(string clientId, bool isIosDevice)
        {
            var assetsGroupsForClient = (await _tableStorage.GetDataAsync(AssetGroupEntity.GroupClientLink.GeneratePartitionKey(clientId))).ToArray();
            var clientIsNotAssignedToAnyIosGroup = !assetsGroupsForClient.Any(x => x.IsIosDevice);
            var clientIsNotAssignedToAnyOtherGroup = assetsGroupsForClient.All(x => x.IsIosDevice);

            return (isIosDevice && clientIsNotAssignedToAnyIosGroup) ||
                assetsGroupsForClient.Any(p => p.ClientsCanCashInViaBankCards && p.IsIosDevice == isIosDevice) ||
                (!isIosDevice && clientIsNotAssignedToAnyOtherGroup);
        }

        public async Task<bool> SwiftDepositEnabledForClient(string clientId, bool isIosDevice)
        {
            var assetsGroupsForClient = (await _tableStorage.GetDataAsync(AssetGroupEntity.GroupClientLink.GeneratePartitionKey(clientId))).ToArray();

            var clientIsNotAssignedToAnyIosGroup = !assetsGroupsForClient.Any(x => x.IsIosDevice);
            var clientIsNotAssignedToAnyOtherGroup = assetsGroupsForClient.All(x => x.IsIosDevice);

            return (isIosDevice && clientIsNotAssignedToAnyIosGroup) ||
                assetsGroupsForClient.Any(p => p.SwiftDepositEnabled && p.IsIosDevice == isIosDevice) ||
                (!isIosDevice && clientIsNotAssignedToAnyOtherGroup);
        }

        public async Task<IEnumerable<IAssetGroup>> GetAllAsync()
        {
            return await GetAllGroups();
        }
    }
}
