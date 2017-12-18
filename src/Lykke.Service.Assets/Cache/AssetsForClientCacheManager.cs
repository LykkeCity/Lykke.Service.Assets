using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;
using Microsoft.Extensions.Caching.Distributed;
using StackExchange.Redis;

namespace Lykke.Service.Assets.Cache
{
    public class AssetsForClientCacheManager : IAssetsForClientCacheManager
    {
        private const string PatternClient = ":Assets:Client:";

        private readonly IDistributedCache _cache;
        private readonly IAssetsForClientCacheManagerSettings _settings;
        private readonly IServer _redisServer;
        private readonly IDatabase _redisDatabase;
        private readonly ILog _log;

        public AssetsForClientCacheManager(
            IDistributedCache cache, 
            IAssetsForClientCacheManagerSettings settings,
            IServer redisServer, 
            IDatabase redisDatabase,
            ILog log)
        {
            _cache = cache;
            _settings = settings;
            _redisServer = redisServer;
            _redisDatabase = redisDatabase;
            _log = log;
        }

        public async Task ClearCacheAsync(string reason)
        {
            RedisKey[] keys = _redisServer.Keys(pattern: $"{_settings.InstanceName}{PatternClient}*", pageSize: 1000).ToArray();

            await _redisDatabase.KeyDeleteAsync(keys);
            
            await _log.WriteInfoAsync(nameof(AssetsForClientCacheManager), nameof(ClearCacheAsync), $"Clear assets cache, count of record: {keys.Length}, reason: {reason}");
        }

        public async Task RemoveClientFromCacheAsync(string clientId)
        {
            try
            {
                await Task.WhenAll(
                    _cache.RemoveAsync(GetKeyAvailableAssets(clientId, true)),
                    _cache.RemoveAsync(GetKeyAvailableAssets(clientId, false)),
                    _cache.RemoveAsync(GetKeyCashInViaBankCardEnabled(clientId, true)),
                    _cache.RemoveAsync(GetKeyCashInViaBankCardEnabled(clientId, false)),
                    _cache.RemoveAsync(GetKeySwiftDepositEnabled(clientId, true)),
                    _cache.RemoveAsync(GetKeySwiftDepositEnabled(clientId, false)),
                    _cache.RemoveAsync(GetKeyAssetConditions(clientId)));
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(RemoveClientFromCacheAsync), clientId, exception);
            }
        }

        public async Task SaveAssetForClientAsync(string clientId, bool isIosDevice, IEnumerable<string> clientAssetIds)
            => await SetAsync(GetKeyAvailableAssets(clientId, isIosDevice), clientId, clientAssetIds.ToList());

        public async Task SaveCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice, bool enabled)
            => await SetAsync(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice), clientId, enabled);

        public async Task SaveSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice, bool enabled)
            => await SetAsync(GetKeySwiftDepositEnabled(clientId, isIosDevice), clientId, enabled);

        public async Task SaveAssetConditionsForClientAsync(string clientId, IList<IAssetCondition> conditions)
            => await SetAsync(GetKeyAssetConditions(clientId), clientId, conditions);

        public async Task<IReadOnlyList<string>> TryGetAssetForClientAsync(string clientId, bool isIosDevice)
            => await TryGetAsync<List<string>>(GetKeyAvailableAssets(clientId, isIosDevice), clientId);

        public async Task<bool?> TryGetCashInViaBankCardEnabledForClientAsync(string clientId, bool isIosDevice)
            => await TryGetAsync<bool?>(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice), clientId);

        public async Task<bool?> TryGetSwiftDepositEnabledForClientAsync(string clientId, bool isIosDevice)
            => await TryGetAsync<bool?>(GetKeySwiftDepositEnabled(clientId, isIosDevice), clientId);

        public async Task<IList<IAssetCondition>> TryGetAssetConditionsForClientAsync(string clientId)
        {
            var conditons = await TryGetAsync<List<AssetCondition>>(GetKeyAssetConditions(clientId), clientId);
            return conditons?.Cast<IAssetCondition>().ToList();
        }

        public async Task SetAsync<T>(string key, string context, T value)
        {
            try
            {
                await _cache.SetStringAsync(key, value.ToJson(),
                    new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = _settings.AssetsForClientCacheTimeSpan
                    });
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(SetAsync), context, exception);
            }
        }

        private async Task<T> TryGetAsync<T>(string key, string context)
        {
            try
            {
                string value = await _cache.GetStringAsync(key);

                if (!string.IsNullOrEmpty(value))
                {
                    return value.DeserializeJson<T>();
                }
            }
            catch (Exception exception)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(TryGetAsync), context, exception);
            }
            
            return default(T);
        }

        private static string GetKeyAssetConditions(string clientId)
            => $"{PatternClient}AssetConditions:{clientId}";

        private static string GetKeyAvailableAssets(string clientId, bool isIosDevice)
            => GetKey("AvailableAssets", clientId, isIosDevice);

        private static string GetKeyCashInViaBankCardEnabled(string clientId, bool isIosDevice)
            => GetKey("CashInViaBankCard", clientId, isIosDevice);

        private static string GetKeySwiftDepositEnabled(string clientId, bool isIosDevice)
            => GetKey("SwiftDeposit", clientId, isIosDevice);

        private static string GetKey(string key, string clientId, bool isIosDevice)
            => $"{PatternClient}{key}:{clientId}" + (isIosDevice ? "_ios_device" : string.Empty);
    }
}
