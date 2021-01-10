using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MongoDB.Bson.IO;
using JsonConvert = Newtonsoft.Json.JsonConvert;

namespace Lykke.Service.Assets.Cache
{
    [UsedImplicitly]
    public class AssetsForClientCacheManager : IAssetsForClientCacheManager
    {
        private readonly IServer _redisServer;
        //private readonly IDatabase _redisDatabase;
        private readonly ILog _log;
        private readonly string _partitionKey;
        private readonly TimeSpan _expiration;

        public AssetsForClientCacheManager(
            IServer redisServer,
            IDatabase redisDatabase,
            ILogFactory logFactory,
            string partitionKey, TimeSpan expiration)
        {
            _redisServer = redisServer;
            //_redisDatabase = redisDatabase;
            _expiration = expiration;
            _log = logFactory.CreateLog(this);
            _partitionKey = partitionKey;
        }

        public async Task ClearCacheAsync(string reason)
        {
            RedisKey[] keys = _redisServer.Keys(pattern: $"{_partitionKey}*", pageSize: 1000).ToArray();

            //await _redisDatabase.KeyDeleteAsync(keys);
            lock (_data)
            {
                _data.Clear();
            }

            _log.Info($"Clear assets cache, count of record: {keys.Length}, reason: {reason}");
        }

        public async Task RemoveClientFromCacheAsync(string clientId)
        {
            try
            {
                lock (_data)
                {
                    _data.Remove(GetKeyCashInViaBankCardEnabled(clientId, true));
                    _data.Remove(GetKeyCashInViaBankCardEnabled(clientId, false));
                    _data.Remove(GetKeySwiftDepositEnabled(clientId, true));
                    _data.Remove(GetKeySwiftDepositEnabled(clientId, false));
                    _data.Remove(GetKeyAssetConditions(clientId));
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }
        }

        public Task SaveCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice, bool enabled)
            => SetAsync(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice), enabled);

        public Task SaveSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice, bool enabled)
            => SetAsync(GetKeySwiftDepositEnabled(clientId, isIosDevice), enabled);

        public Task SaveAssetConditionsForClientAsync(string clientId, IList<IAssetCondition> conditions)
            => SetAsync(GetKeyAssetConditions(clientId), conditions);

        public Task<bool?> TryGetCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice)
            => TryGetAsync<bool?>(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice));

        public Task<bool?> TryGetSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice)
            => TryGetAsync<bool?>(GetKeySwiftDepositEnabled(clientId, isIosDevice));

        public async Task<IList<IAssetCondition>> TryGetAssetConditionsForClientAsync(string clientId)
        {
            var conditons = await TryGetAsync<List<AssetCondition>>(GetKeyAssetConditions(clientId));
            return conditons?.Cast<IAssetCondition>().ToList();
        }

        private static Dictionary<string, string> _data = new Dictionary<string, string>();

        private async Task SetAsync<T>(string key, T value)
        {
            try
            {
                lock (_data)
                {
                    _data[key] = value.ToJson();
                }
                //await _redisDatabase.StringSetAsync(key, value.ToJson(), _expiration);
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }
        }

        private async Task<T> TryGetAsync<T>(string key)
        {
            try
            {
                lock (_data)
                {
                    if (_data.TryGetValue(key, out var result))
                    {
                        var resp = JsonConvert.DeserializeObject<T>(result);
                        
                        return resp;
                    }
                }

                //string value = await _redisDatabase.StringGetAsync(key);

                //if (!string.IsNullOrEmpty(value))
                //{
                //    return value.DeserializeJson<T>();
                //}
            }
            catch (Exception exception)
            {
                Console.WriteLine($"Error! key: {key}, T: {typeof(T).Name}");
                _log.Error(exception);
            }

            return default(T);
        }

        private string GetKeyAssetConditions(string clientId)
            => $"{_partitionKey}:AssetConditions:{clientId}";
        
        private string GetKeyCashInViaBankCardEnabled(string clientId, bool isIosDevice)
            => GetKey("CashInViaBankCard", clientId, isIosDevice);

        private string GetKeySwiftDepositEnabled(string clientId, bool isIosDevice)
            => GetKey("SwiftDeposit", clientId, isIosDevice);

        private string GetKey(string key, string clientId, bool isIosDevice)
            => $"{_partitionKey}:{key}:{clientId}" + (isIosDevice ? "_ios_device" : string.Empty);
    }
}
