using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;
using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetGroupEntity : TableEntity, IAssetGroup
    {
        public string Name { get; set; }
        public bool IsIosDevice { get; set; }
        public string AssetId { get; set; }
        public string ClientId { get; set; }
        public bool ClientsCanCashInViaBankCards { get; set; }
        public bool SwiftDepositEnabled { get; set; }
        public string Id => RowKey;

        public static class Record
        {
            public static string GenerateRowKey(string group)
            {
                return group;
            }

            public static string GeneratePartitionKey()
            {
                return "AssetGroup";
            }

            public static AssetGroupEntity Create(string group, bool isIosDevice, bool clientsCanCashInViaBankCards,
                bool swiftDepositEnabled)
            {
                return new AssetGroupEntity
                {
                    RowKey = GenerateRowKey(group),
                    PartitionKey = GeneratePartitionKey(),
                    Name = group,
                    IsIosDevice = isIosDevice,
                    ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards,
                    SwiftDepositEnabled = swiftDepositEnabled
                };
            }

            public static AssetGroupEntity Create(IAssetGroup assetGroup)
            {
                return new AssetGroupEntity
                {
                    RowKey = GenerateRowKey(assetGroup.Name),
                    PartitionKey = GeneratePartitionKey(),
                    Name = assetGroup.Name,
                    IsIosDevice = assetGroup.IsIosDevice,
                    ClientsCanCashInViaBankCards = assetGroup.ClientsCanCashInViaBankCards,
                    SwiftDepositEnabled = assetGroup.SwiftDepositEnabled
                };
            }
        }

        public static class ClientGroupLink
        {
            public static string GenerateRowKey(string clientId)
            {
                return clientId;
            }

            public static string GeneratePartitionKey(string group)
            {
                return $"ClientGroupLink_{group}";
            }

            public static AssetGroupEntity Create(string group, string clientId, bool isIosDevice, bool clientsCanCashInViaBankCards,
                bool swiftDepositEnabled)
            {
                return new AssetGroupEntity
                {
                    RowKey = GenerateRowKey(clientId),
                    PartitionKey = GeneratePartitionKey(group),
                    Name = group,
                    ClientId = clientId,
                    IsIosDevice = isIosDevice,
                    ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards,
                    SwiftDepositEnabled = swiftDepositEnabled
                };
            }

            public static void Update(AssetGroupEntity entity, IAssetGroup assetGroup)
            {
                entity.Name = assetGroup.Name;
                entity.ClientsCanCashInViaBankCards = assetGroup.ClientsCanCashInViaBankCards;
                entity.SwiftDepositEnabled = assetGroup.SwiftDepositEnabled;
                entity.IsIosDevice = assetGroup.IsIosDevice;
            }
        }

        public static class GroupClientLink
        {
            public static string GenerateRowKey(string group)
            {
                return group;
            }

            public static string GeneratePartitionKey(string clientId)
            {
                return $"GroupClientLink_{clientId}";
            }

            public static AssetGroupEntity Create(string group, string clientId, bool isIosDevice, bool clientsCanCashInViaBankCards,
                bool swiftDepositEnabled)
            {
                return new AssetGroupEntity
                {
                    RowKey = GenerateRowKey(group),
                    PartitionKey = GeneratePartitionKey(clientId),
                    Name = group,
                    ClientId = clientId,
                    IsIosDevice = isIosDevice,
                    ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards,
                    SwiftDepositEnabled = swiftDepositEnabled
                };
            }

            public static void Update(AssetGroupEntity entity, IAssetGroup assetGroup)
            {
                entity.Name = assetGroup.Name;
                entity.ClientsCanCashInViaBankCards = assetGroup.ClientsCanCashInViaBankCards;
                entity.SwiftDepositEnabled = assetGroup.SwiftDepositEnabled;
                entity.IsIosDevice = assetGroup.IsIosDevice;
            }
        }

        public static class AssetLink
        {
            public static string GenerateRowKey(string assetId)
            {
                return assetId;
            }

            public static string GeneratePartitionKey(string group)
            {
                return $"AssetLink_{group}";
            }

            public static AssetGroupEntity Create(string group, string assetId, bool isIosDevice, bool clientsCanCashInViaBankCards,
                bool swiftDepositEnabled)
            {
                return new AssetGroupEntity
                {
                    RowKey = GenerateRowKey(assetId),
                    PartitionKey = GeneratePartitionKey(group),
                    Name = group,
                    AssetId = assetId,
                    IsIosDevice = isIosDevice,
                    ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards,
                    SwiftDepositEnabled = swiftDepositEnabled
                };
            }
        }
    }
}
