using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using StackExchange.Redis;

namespace Lykke.Service.Assets.Cache
{
    [UsedImplicitly]
    public class AssetsForClientCacheManager : IAssetsForClientCacheManager
    {
        private const string PatternClient = ":v2:Assets:Client:";

        private readonly IAssetsForClientCacheManagerSettings _settings;
        private readonly IServer _redisServer;
        private readonly IDatabase _redisDatabase;
        private readonly ILog _log;

        public AssetsForClientCacheManager(
            IAssetsForClientCacheManagerSettings settings,
            IServer redisServer,
            IDatabase redisDatabase,
            ILogFactory logFactory)
        {
            _settings = settings;
            _redisServer = redisServer;
            _redisDatabase = redisDatabase;
            _log = logFactory.CreateLog(this);
        }

        public async Task ClearCacheAsync(string reason)
        {
            RedisKey[] keys = _redisServer.Keys(pattern: $"{_settings.InstanceName}{PatternClient}*", pageSize: 1000).ToArray();

            await _redisDatabase.KeyDeleteAsync(keys);
            
            _log.Info($"Clear assets cache, count of record: {keys.Length}, reason: {reason}");
        }

        public async Task RemoveClientFromCacheAsync(string clientId)
        {
            try
            {
                await Task.WhenAll(
                    _redisDatabase.KeyDeleteAsync(GetKeyAvailableAssets(clientId, true)),
                    _redisDatabase.KeyDeleteAsync(GetKeyAvailableAssets(clientId, false)),
                    _redisDatabase.KeyDeleteAsync(GetKeyCashInViaBankCardEnabled(clientId, true)),
                    _redisDatabase.KeyDeleteAsync(GetKeyCashInViaBankCardEnabled(clientId, false)),
                    _redisDatabase.KeyDeleteAsync(GetKeySwiftDepositEnabled(clientId, true)),
                    _redisDatabase.KeyDeleteAsync(GetKeySwiftDepositEnabled(clientId, false)),
                    _redisDatabase.KeyDeleteAsync(GetKeyAssetConditions(clientId)));
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }
        }

        public Task SaveCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice, bool enabled)
            => SetAsync(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice), clientId, enabled);

        public Task SaveSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice, bool enabled)
            => SetAsync(GetKeySwiftDepositEnabled(clientId, isIosDevice), clientId, enabled);

        public Task SaveAssetConditionsForClientAsync(string clientId, IList<IAssetCondition> conditions)
            => SetAsync(GetKeyAssetConditions(clientId), clientId, conditions);

        public Task<bool?> TryGetCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice)
            => TryGetAsync<bool?>(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice), clientId);

        public Task<bool?> TryGetSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice)
            => TryGetAsync<bool?>(GetKeySwiftDepositEnabled(clientId, isIosDevice), clientId);

        public async Task<IList<IAssetCondition>> TryGetAssetConditionsForClientAsync(string clientId)
        {
            var conditons = await TryGetAsync<List<AssetCondition>>(GetKeyAssetConditions(clientId), clientId);
            return conditons?.Cast<IAssetCondition>().ToList();
        }

        private async Task SetAsync<T>(string key, string context, T value)
        {
            try
            {
                await _redisDatabase.StringSetAsync(key, value.ToJson(), _settings.AssetsForClientCacheTimeSpan);
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }
        }

        private async Task<T> TryGetAsync<T>(string key, string context)
        {
            try
            {
                string value = await _redisDatabase.StringGetAsync(key);

                if (!string.IsNullOrEmpty(value))
                {
                    return value.DeserializeJson<T>();
                }
            }
            catch (Exception exception)
            {
                _log.Error(exception);
            }

            return default(T);
        }

        private string GetKeyAssetConditions(string clientId)
            => $"{_settings.InstanceName}{PatternClient}AssetConditions:{clientId}";

        private string GetKeyAvailableAssets(string clientId, bool isIosDevice)
            => GetKey("AvailableAssets", clientId, isIosDevice);

        private string GetKeyCashInViaBankCardEnabled(string clientId, bool isIosDevice)
            => GetKey("CashInViaBankCard", clientId, isIosDevice);

        private string GetKeySwiftDepositEnabled(string clientId, bool isIosDevice)
            => GetKey("SwiftDeposit", clientId, isIosDevice);

        private string GetKey(string key, string clientId, bool isIosDevice)
            => $"{_settings.InstanceName}{PatternClient}{key}:{clientId}" + (isIosDevice ? "_ios_device" : string.Empty);
    }
}
