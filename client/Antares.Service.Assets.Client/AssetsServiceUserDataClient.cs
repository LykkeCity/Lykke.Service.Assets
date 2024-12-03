using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Antares.Service.Assets.Client.Models;
using Autofac;
using Common.Log;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.NoSql.Models;
using MyNoSqlServer.Abstractions;
using MyNoSqlServer.DataReader;

namespace Antares.Service.Assets.Client
{
    public class AssetsServiceUserDataClient: IStartable, IDisposable, IAssetsServiceUserDataClient, IWatchListsClient, IAvailableAssetClient
    {
        private readonly MyNoSqlTcpClient _myNoSqlClient;
        private readonly AssetsServiceHttp _httpClient;

        private readonly IMyNoSqlServerDataReader<AssetConditionNoSql> _readerAssetConditionNoSql;
        private readonly IMyNoSqlServerDataReader<WatchListCustomNoSql> _readerWatchListCustomNoSql;
        private MyNoSqlReadRepository<WatchListPredefinedNoSql> _readerWatchListPredefinedNoSql;
        private ILog _log;


        public AssetsServiceUserDataClient(string myNoSqlServerReaderHostPort, string assetServiceHttpApiUrl, ILogFactory logFactory)
        {
            _log = logFactory.CreateLog(this);
            var host = Environment.GetEnvironmentVariable("HOST") ?? Environment.MachineName;
            _httpClient = new AssetsServiceHttp(new Uri(assetServiceHttpApiUrl));

            _myNoSqlClient = new MyNoSqlTcpClient(() => myNoSqlServerReaderHostPort, host);
            _readerAssetConditionNoSql = new MyNoSqlReadRepository<AssetConditionNoSql>(_myNoSqlClient, AssetConditionNoSql.TableName);
            _readerWatchListCustomNoSql = new MyNoSqlReadRepository<WatchListCustomNoSql>(_myNoSqlClient, WatchListCustomNoSql.TableNameCustomWatchList);
            _readerWatchListPredefinedNoSql = new MyNoSqlReadRepository<WatchListPredefinedNoSql>(_myNoSqlClient, WatchListPredefinedNoSql.TableNamePredefinedWatchList);
        }

        public void Start()
        {
            _myNoSqlClient.Start();

            var sw = new Stopwatch();
            sw.Start();
            var iteration = 0;
            while (iteration < 100)
            {
                iteration++;
                if (_readerWatchListPredefinedNoSql.Count() > 0)
                    break;

                Thread.Sleep(100);
            }
            sw.Stop();
            Console.WriteLine($"AssetsServiceUserDataClient client is started. Wait time: {sw.ElapsedMilliseconds} ms");
        }

        public void Dispose()
        {
            _myNoSqlClient.Stop();
        }

        public IWatchListsClient WatchLists => this;
        public IAvailableAssetClient AvailableAssets => this;
        public IAssetsServiceHttp HttpClient => _httpClient;

        async Task<List<string>> IAvailableAssetClient.GetAssetIds(string clientId, bool isIosDevice)
        {
            try
            {
                var data = _readerAssetConditionNoSql.Get(
                    AssetConditionNoSql.GeneratePartitionKey(clientId),
                    AssetConditionNoSql.GenerateRowKey());

                if (data?.AssetConditions != null)
                {
                    return data.AssetConditions.Where(o => o.AvailableToClient == true).Select(o => o.Asset).ToList();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot read from MyNoSQL. Table: ${AssetConditionNoSql.TableName}, PK: {AssetConditionNoSql.GeneratePartitionKey(clientId)}, RK: {AssetConditionNoSql.GenerateRowKey()}");
                throw;
            }

            var result = await HttpClient.ClientGetAssetIdsAsync(clientId, isIosDevice);
            return result.ToList();
        }

        async Task<IWatchList> IWatchListsClient.AddCustomAsync(WatchListDto watchList, string clientId)
        {
            var result = await HttpClient.WatchListAddCustomAsync(FromWatchListDto(watchList), clientId);
            var dto = FromWatchListResponse(result);
            return dto;
        }

        async Task IWatchListsClient.UpdateCustomWatchListAsync(string clientId, WatchListDto watchList)
        {
            await HttpClient.WatchListUpdateCustomAsync(FromWatchListDto(watchList), clientId);
        }

        async Task IWatchListsClient.RemoveCustomAsync(string watchListId, string clientId)
        {
            await HttpClient.WatchListCustomRemoveWithHttpMessagesAsync(watchListId, clientId);
        }

        async Task<IWatchList> IWatchListsClient.AddPredefinedAsync(WatchListDto watchList)
        {
            var result = await HttpClient.WatchListAddPredefinedAsync(FromWatchListDto(watchList));
            var data = FromWatchListResponse(result);
            return data;
        }

        async Task IWatchListsClient.UpdatePredefinedAsync(WatchListDto watchList)
        {
            await HttpClient.WatchListUpdatePredefinedAsync(FromWatchListDto(watchList));
        }


        async Task<IWatchList> IWatchListsClient.GetCustomWatchListAsync(string clientId, string watchListId)
        {
            try
            {
                var data = _readerWatchListCustomNoSql.Get(WatchListCustomNoSql.GeneratePartitionKey(clientId), WatchListCustomNoSql.GenerateRowKey(watchListId));

                if (data != null)
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot read from MyNoSQL. Table: ${WatchListCustomNoSql.TableNameCustomWatchList}, PK: {WatchListCustomNoSql.GeneratePartitionKey(clientId)}", ex);
            }

            try
            {
                var result = await HttpClient.WatchListGetCustomAsync(watchListId, clientId);
                var data = FromWatchListResponse(result);
                return data;
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot read from API. Method: WatchListGetCustomAsync, clientId: {clientId}, watchListId: {watchListId}");
                throw;
            }
        }

        async Task<IWatchList> IWatchListsClient.GetPredefinedWatchListAsync(string watchListId)
        {
            try
            {
                var data = _readerWatchListCustomNoSql.Get(WatchListPredefinedNoSql.GeneratePartitionKey(), WatchListPredefinedNoSql.GenerateRowKey(watchListId));

                if (data != null)
                {
                    return data;
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot read from MyNoSQL. Table: ${WatchListPredefinedNoSql.TableNamePredefinedWatchList}, PK: {WatchListPredefinedNoSql.GeneratePartitionKey()}, RK: {WatchListPredefinedNoSql.GenerateRowKey(watchListId)}", ex);
            }

            try
            {
                var result = await HttpClient.WatchListGetPredefinedAsync(watchListId);
                var data = FromWatchListResponse(result);
                return data;
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot read from API. Method: WatchListGetPredefinedAsync, watchListId: {watchListId}");
                throw;
            }
        }

        async Task<List<IWatchList>> IWatchListsClient.GetAllCustom(string clientId)
        {
            try
            {
                var data = _readerWatchListCustomNoSql.Get(WatchListCustomNoSql.GeneratePartitionKey(clientId));

                if (data != null)
                {
                    return data.Select(e => (IWatchList)e).ToList();
                }
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot read from MyNoSQL. Table: ${WatchListCustomNoSql.TableNameCustomWatchList}, PK: {WatchListCustomNoSql.GeneratePartitionKey(clientId)}", ex);
            }

            try
            {
                var result = await HttpClient.WatchListGetAllCustomAsync(clientId);
                var resultData = result.Select(e => (IWatchList) FromWatchListResponse(e)).ToList();
                return resultData;
            }
            catch (Exception ex)
            {
                _log.Error(ex, $"Cannot read from API. Method: WatchListGetAllCustomAsync, clientId: {clientId}");
                throw;
            }
        }

        private WatchListDto FromWatchListResponse(WatchList item)
        {
            if (item == null)
                return null;


            return new WatchListDto()
            {
                Id = item.Id,
                Name = item.Name,
                Order = item.Order,
                ReadOnly = item.ReadOnlyProperty,
                AssetIds = item.AssetIds.ToList()
            };
        }

        private WatchList FromWatchListDto(WatchListDto item)
        {
            if (item == null)
                return null;

            return new WatchList(item.AssetIds, item.Id, item.Name, item.Order, item.ReadOnly);
        }

        
    }
}
