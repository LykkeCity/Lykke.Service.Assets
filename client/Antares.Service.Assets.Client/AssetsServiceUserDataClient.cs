using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Antares.Service.Assets.Client.Models;
using Autofac;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.DataReader;

namespace Antares.Service.Assets.Client
{
    public class AssetsServiceUserDataClient: IStartable, IDisposable, IAssetsServiceUserDataClient, IWatchListsClient
    {
        private readonly MyNoSqlTcpClient _myNoSqlClient;
        private readonly AssetsServiceHttp _httpClient;

        public AssetsServiceUserDataClient(string myNoSqlServerReaderHostPort, string assetServiceHttpApiUrl)
        {
            var host = Environment.GetEnvironmentVariable("HOST") ?? Environment.MachineName;
            _httpClient = new AssetsServiceHttp(new Uri(assetServiceHttpApiUrl));

            _myNoSqlClient = new MyNoSqlTcpClient(() => myNoSqlServerReaderHostPort, host);
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
                //if (Assets.GetAll().Count > 0 && AssetExtendedInfo.GetAll().Count > 0 && AssetPairs.GetAll().Count > 0)
                break;

                Thread.Sleep(100);
            }
            sw.Stop();
            Console.WriteLine($"AssetsWatchLists client is started. Wait time: {sw.ElapsedMilliseconds} ms");
        }

        public void Dispose()
        {
            _myNoSqlClient.Stop();
        }

        public IWatchListsClient WatchLists => this;
        public IAssetsServiceHttp HttpClient => _httpClient;

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

        async Task<IEnumerable<IWatchList>> IWatchListsClient.GetAllCustom(string clientId)
        {
            var result = await HttpClient.WatchListGetAllCustomAsync(clientId);
            var data = result.Select(e => (IWatchList) FromWatchListResponse(e)).ToList();
            return data;
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
            var result = await HttpClient.WatchListGetCustomAsync(watchListId, clientId);
            var data = FromWatchListResponse(result);
            return data;
        }

        async Task<IWatchList> IWatchListsClient.GetPredefinedWatchListAsync(string watchListId)
        {
            var result = await HttpClient.WatchListGetPredefinedAsync(watchListId);
            var data = FromWatchListResponse(result);
            return data;
        }

        private WatchListDto FromWatchListResponse(WatchList item)
        {
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
            return new WatchList(item.AssetIds, item.Id, item.Name, item.Order, item.ReadOnly);
        }
    }
}
