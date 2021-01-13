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
using Lykke.Service.Assets.NoSql.Models.AssetAttributeModel;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;

namespace Antares.Service.Assets.Client
{
    public partial class AssetsServiceClient : IAssetsServiceClient, IAssetAttributesClient, IStartable, IDisposable
    {
        private readonly MyNoSqlTcpClient _myNoSqlClient;

        private readonly IMyNoSqlServerDataReader<AssetAttributeNoSql> _readerAssetAttributeNoSql;
        private readonly IMyNoSqlServerDataReader<AssetCategoryNoSql> _readerAssetCategoryNoSql;
        private readonly AssetsServiceHttp _httpClient;

        public AssetsServiceClient(
            string myNoSqlServerReaderHostPort, 
            string assetServiceHttpApiUrl, 
            ILogFactory logFactory)
        {
            var host = Environment.GetEnvironmentVariable("HOST") ?? Environment.MachineName;
            _httpClient = new AssetsServiceHttp(new Uri(assetServiceHttpApiUrl), new HttpClient());

            _myNoSqlClient = new MyNoSqlTcpClient(() => myNoSqlServerReaderHostPort,host);
            
            _readerAssetAttributeNoSql = new MyNoSqlReadRepository<AssetAttributeNoSql>(_myNoSqlClient, AssetAttributeNoSql.TableName);
            _readerAssetCategoryNoSql = new MyNoSqlReadRepository<AssetCategoryNoSql>(_myNoSqlClient, AssetCategoryNoSql.TableName);
        }

        public IAssetAttributesClient AssetAttributes => this;
        public IAssetCategoryClient AssetCategory => this;

        public IAssetsServiceHttp HttpClient => _httpClient;

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
