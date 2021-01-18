using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.NoSql.Models;

namespace Antares.Service.Assets.Client
{
    public partial class AssetsServiceClient : IAssetExtendedInfoClient
    {
        IAssetExtendedInfo IAssetExtendedInfoClient.Get(string id)
        {
            try
            {
                var data = _readerAssetExtendedInfoNoSql.Get(
                    AssetExtendedInfoNoSql.GeneratePartitionKey(),
                    AssetExtendedInfoNoSql.GenerateRowKey(id));

                return data?.ExtendedInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetExtendedInfoNoSql.TableName}, PK: {AssetExtendedInfoNoSql.GeneratePartitionKey()}, RK: {AssetExtendedInfoNoSql.GenerateRowKey(id)}, Ex: {ex}");
                throw;
            }
        }

        IList<IAssetExtendedInfo> IAssetExtendedInfoClient.GetAll()
        {
            try
            {
                var data = _readerAssetExtendedInfoNoSql
                    .Get(AssetExtendedInfoNoSql.GeneratePartitionKey())
                    .Select(e => (IAssetExtendedInfo)e.ExtendedInfo).ToList();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetExtendedInfoNoSql.TableName}, Ex: {ex}");
                throw;
            }
        }

        IAssetExtendedInfo IAssetExtendedInfoClient.GetDefault()
        {
            try
            {
                var data = _readerAssetExtendedInfoNoSql.Get(
                    AssetExtendedInfoNoSql.DefaultInfoPartitionKey,
                    AssetExtendedInfoNoSql.DefaultInfoRowKey);

                return data?.ExtendedInfo;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetExtendedInfoNoSql.TableName}, PK: {AssetExtendedInfoNoSql.DefaultInfoPartitionKey}, RK: {AssetExtendedInfoNoSql.DefaultInfoRowKey}, Ex: {ex}");
                throw;
            }
        }
    }
}
