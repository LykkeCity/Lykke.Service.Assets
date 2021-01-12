using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Autofac;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.NoSql.Models.AssetAttributeModel;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;

namespace Antares.Service.Assets.Client
{
    public interface IAssetsServiceClient
    {
        IAssetAttributesClient AssetAttributes { get; }

        IAssetsServiceHttp HttpClient { get; }
    }


    public interface IAssetAttributesClient
    {
        Task<IAssetAttribute> Get(string assetId, string key);

        Task<IList<IAssetAttributes>> GetAll();

        Task<IAssetAttributes> GetAllForAsset(string assetId);
    }


    public class AssetsServiceClient : IAssetsServiceClient, IAssetAttributesClient, IStartable, IDisposable
    {
        private readonly MyNoSqlTcpClient _myNoSqlClient;

        private readonly IMyNoSqlServerDataReader<AssetAttributeNoSql> _readerAssetAttributeNoSql;
        private readonly AssetsServiceHttp _httpClient;

        public AssetsServiceClient(string myNoSqlServerReaderHostPort, string assetServiceHttpApiUrl, ILogFactory logFactory)
        {
            var host = Environment.GetEnvironmentVariable("HOST") ?? Environment.MachineName;

            _myNoSqlClient = new MyNoSqlTcpClient(() => myNoSqlServerReaderHostPort,host);
            _readerAssetAttributeNoSql = new MyNoSqlReadRepository<AssetAttributeNoSql>(_myNoSqlClient, AssetAttributeNoSql.TableName);

            _httpClient = new AssetsServiceHttp(new Uri(assetServiceHttpApiUrl), new HttpClient());
        }

        public IAssetAttributesClient AssetAttributes
        {
            get { return this; }
        }

        public IAssetsServiceHttp HttpClient => _httpClient;

        async Task<IAssetAttribute> IAssetAttributesClient.Get(string assetId, string key)
        {
            try
            {
                var data = _readerAssetAttributeNoSql.Get(AssetAttributeNoSql.GeneratePartitionKey(assetId), AssetAttributeNoSql.GenerateRowKey(key));
                return data;
            }
            catch(Exception ex)
            {
                Console.WriteLine("Cannot read data from MyNoSQL: " + ex);
            }

            var resp = await _httpClient.AssetAttributeGetAsync(assetId, key);

            return new AssetAttributeDto()
            {
                Key = key,
                Value = resp.Value
            };
        }

        async Task<IList<IAssetAttributes>> IAssetAttributesClient.GetAll()
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
                Console.WriteLine("Cannot read data from MyNoSQL: " + ex);
            }

            {
                var data = await _httpClient.AssetAttributeGetAllAsync();
                var result = data
                    .Select(e => (IAssetAttributes)new AssetAttributesDto()
                    {
                        AssetId = e.AssetId,
                        Attributes = e.Attributes.Select(a => (IAssetAttribute) new AssetAttributeDto() {Key = a.Key, Value = a.Value}).ToList()
                    })
                    .ToList();

                return result;
            }
        }

        async Task<IAssetAttributes> IAssetAttributesClient.GetAllForAsset(string assetId)
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
                Console.WriteLine("Cannot read data from MyNoSQL: " + ex);
            }

            {
                var data = await _httpClient.AssetAttributeGetAllForAssetAsync(assetId);
                var result = (IAssetAttributes)new AssetAttributesDto()
                {
                    AssetId = assetId,
                    Attributes = data.Attributes.Select(a => (IAssetAttribute)new AssetAttributeDto() { Key = a.Key, Value = a.Value }).ToList()
                };

                return result;
            }
        }

        public void Start()
        {
            _myNoSqlClient.Start();
        }

        public void Dispose()
        {
            _myNoSqlClient.Stop();
        }
    }
}
