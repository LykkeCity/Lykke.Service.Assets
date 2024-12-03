using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.NoSql.Models;

namespace Antares.Service.Assets.Client
{
    public partial class AssetsServiceClient : IAssetPairsClient
    {
        IAssetPair IAssetPairsClient.Get(string id)
        {
            try
            {
                var data = _readerAssetPairNoSql.Get(
                    AssetPairNoSql.GeneratePartitionKey(),
                    AssetPairNoSql.GenerateRowKey(id));

                return data?.AssetPair;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetPairNoSql.TableName}, PK: {AssetPairNoSql.GeneratePartitionKey()}, RK: {AssetPairNoSql.GenerateRowKey(id)}, Ex: {ex}");
                throw;
            }
        }

        List<IAssetPair> IAssetPairsClient.GetAll()
        {
            try
            {
                var data = _readerAssetPairNoSql.Get();

                return data.Select(e => (IAssetPair)e.AssetPair).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetPairNoSql.TableName}, Ex: {ex}");
                throw;
            }
        }
    }
}
