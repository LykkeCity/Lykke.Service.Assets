using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Antares.Service.Assets.Client.DTOs;
using Autofac;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.NoSql.Models;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;

namespace Antares.Service.Assets.Client
{
    public partial class AssetsServiceClient 
    {
        IAssetAttribute IAssetAttributesClient.Get(string assetId, string key)
        {
            try
            {
                var data = _readerAssetAttributeNoSql.Get(AssetAttributeNoSql.GeneratePartitionKey(assetId), AssetAttributeNoSql.GenerateRowKey(key));
                return data;
            }
            catch(Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetAttributeNoSql.TableName}, PK: {AssetAttributeNoSql.GeneratePartitionKey(assetId)}, RK: {AssetAttributeNoSql.GenerateRowKey(key)}, Ex: {ex}");
                throw;
            }
        }

        IList<IAssetAttributes> IAssetAttributesClient.GetAll()
        {
            try
            {
                var data = _readerAssetAttributeNoSql.Get();
                var result = data.GroupBy(e => e.AssetId)
                    .Select(e => (IAssetAttributes)new AssetAttributesDto()
                    {
                        AssetId = e.Key, Attributes = e.Select(a => (IAssetAttribute)a).ToList()
                    }).ToList();

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetAttributeNoSql.TableName}, Ex: {ex}");
                throw;
            }
        }

        IAssetAttributes IAssetAttributesClient.GetAllForAsset(string assetId)
        {
            try
            {
                var data = _readerAssetAttributeNoSql.Get(AssetAttributeNoSql.GeneratePartitionKey(assetId));
                
                var result = (IAssetAttributes)new AssetAttributesDto()
                {
                    AssetId = assetId,
                    Attributes = data.Select(a => (IAssetAttribute)a).ToList()
                };

                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Cannot read from MyNoSQL. Table: ${AssetAttributeNoSql.TableName}, PK: {AssetAttributeNoSql.GeneratePartitionKey(assetId)} Ex: {ex}");
                throw;
            }
        }
    }
}
