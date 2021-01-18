using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.NoSql.Models;

namespace Antares.Service.Assets.Client
{
    public partial class AssetsServiceClient : IAssetsClient
    {
        IAsset IAssetsClient.Get(string assetId)
        {
            try
            {
                var data = _readerAssetNoSql.Get(
                    AssetNoSql.GeneratePartitionKey(),
                    AssetNoSql.GenerateRowKey(assetId));

                return data?.Asset;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetNoSql.TableName}, PK: {AssetNoSql.GeneratePartitionKey()}, RK: {AssetNoSql.GenerateRowKey(assetId)}, Ex: {ex}");
                throw;
            }
        }

        List<IAsset> IAssetsClient.GetAll(bool includeNonTradable)
        {
            try
            {
                var data = _readerAssetNoSql.Get().Select(e => (IAsset)e.Asset).Where(e => includeNonTradable || e.IsTradable).ToList();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetNoSql.TableName}, Ex: {ex}");
                throw;
            }
        }

        List<IAsset> IAssetsClient.GetBySpecification(IReadOnlyList<string> ids, bool? isTradable)
        {
            try
            {
                var data = _readerAssetNoSql.Get()
                    .Where(e => ids == null || !ids.Any() || ids.Contains(e.Asset.Id))
                    .Where(e => isTradable == null || e.Asset.IsTradable == isTradable)
                    .Select(e => (IAsset) e.Asset)
                    .ToList();

                return data;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetNoSql.TableName}, Ex: {ex}");
                throw;
            }
        }
    }
}
