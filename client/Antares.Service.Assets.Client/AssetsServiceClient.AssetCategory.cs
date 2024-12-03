using System;
using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.NoSql.Models;

namespace Antares.Service.Assets.Client
{
    public partial class AssetsServiceClient : IAssetCategoryClient
    {
        IAssetCategory IAssetCategoryClient.Get(string id)
        {
            try
            {
                var data = _readerAssetCategoryNoSql.Get(
                    AssetCategoryNoSql.GeneratePartitionKey(), 
                    AssetCategoryNoSql.GenerateRowKey(id));

                return data?.Category;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetCategoryNoSql.TableName}, PK: {AssetCategoryNoSql.GeneratePartitionKey()}, RK: {AssetCategoryNoSql.GenerateRowKey(id)}, Ex: {ex}");
                throw;
            }
        }

        IList<IAssetCategory> IAssetCategoryClient.GetAll()
        {
            try
            {
                var data = _readerAssetCategoryNoSql.Get();

                var result = data.Where(e => e.Category != null).Select(e => (IAssetCategory)e.Category).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetCategoryNoSql.TableName}, Ex: {ex}");
                throw;
            }
        }
    }
}
