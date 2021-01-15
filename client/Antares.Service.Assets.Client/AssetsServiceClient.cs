using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading;
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
    public partial class AssetsServiceClient : IAssetsServiceClient, IAssetAttributesClient, IStartable, IDisposable
    {
        private readonly MyNoSqlTcpClient _myNoSqlClient;

        private readonly IMyNoSqlServerDataReader<AssetAttributeNoSql> _readerAssetAttributeNoSql;
        private readonly IMyNoSqlServerDataReader<AssetCategoryNoSql> _readerAssetCategoryNoSql;
        private readonly IMyNoSqlServerDataReader<AssetExtendedInfoNoSql> _readerAssetExtendedInfoNoSql;
        private readonly IMyNoSqlServerDataReader<AssetNoSql> _readerAssetNoSql;
        private readonly IMyNoSqlServerDataReader<AssetPairNoSql> _readerAssetPairNoSql;
        private readonly AssetsServiceHttp _httpClient;

        public AssetsServiceClient(
            string myNoSqlServerReaderHostPort, 
            string assetServiceHttpApiUrl)
        {
            var host = Environment.GetEnvironmentVariable("HOST") ?? Environment.MachineName;
            _httpClient = new AssetsServiceHttp(new Uri(assetServiceHttpApiUrl));

            _myNoSqlClient = new MyNoSqlTcpClient(() => myNoSqlServerReaderHostPort,host);
            
            _readerAssetAttributeNoSql = new MyNoSqlReadRepository<AssetAttributeNoSql>(_myNoSqlClient, AssetAttributeNoSql.TableName);
            _readerAssetCategoryNoSql = new MyNoSqlReadRepository<AssetCategoryNoSql>(_myNoSqlClient, AssetCategoryNoSql.TableName);
            _readerAssetExtendedInfoNoSql = new MyNoSqlReadRepository<AssetExtendedInfoNoSql>(_myNoSqlClient, AssetExtendedInfoNoSql.TableName);
            _readerAssetNoSql = new MyNoSqlReadRepository<AssetNoSql>(_myNoSqlClient, AssetNoSql.TableName);
            _readerAssetPairNoSql = new MyNoSqlReadRepository<AssetPairNoSql>(_myNoSqlClient, AssetPairNoSql.TableName);
        }

        public IAssetAttributesClient AssetAttributes => this;
        public IAssetCategoryClient AssetCategory => this;
        public IAssetExtendedInfoClient AssetExtendedInfo => this;
        public IAssetPairsClient AssetPairs => this;

        public IAssetsClient Assets => this;

        public IAssetsServiceHttp HttpClient => _httpClient;

        public void Start()
        {
            _myNoSqlClient.Start();

            var sw = new Stopwatch();
            sw.Start();
            var iteration = 0;
            while (iteration < 100)
            {
                iteration++;
                if (Assets.GetAll().Count > 0 && AssetExtendedInfo.GetAll().Count >0 && AssetPairs.GetAll().Count>0)
                    break;

                Thread.Sleep(100);
            }
            sw.Stop();
            Console.WriteLine($"AssetService client is started. Wait time: {sw.ElapsedMilliseconds} ms");
        }

        public void Dispose()
        {
            _myNoSqlClient.Stop();
        }
    }
}
