using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Common;
using Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Services;
using Microsoft.Extensions.Caching.Distributed;

namespace Lykke.Service.Assets.Cache
{
    public class AssetsForClientCacheManager : IAssetsForClientCacheManager
    {
        private readonly IDistributedCache _cache;
        private readonly IAssetsForClientCacheManagerSettings _settings;
        private readonly StackExchange.Redis.IServer _redisServer;
        private readonly StackExchange.Redis.IDatabase _redisDatabase;
        private readonly ILog _log;

        public AssetsForClientCacheManager(IDistributedCache cache, IAssetsForClientCacheManagerSettings settings,
            StackExchange.Redis.IServer redisServer, StackExchange.Redis.IDatabase redisDatabase,
            ILog log)
        {
            _cache = cache;
            _settings = settings;
            _redisServer = redisServer;
            _redisDatabase = redisDatabase;
            _log = log;
        }

        public async Task ClearCache(string reason)
        {
            var count = 0;
            while(true)
            {
                var keys = _redisServer.Keys(pattern: _settings.InstanceName + ":Assets:Client:*", pageSize: 1000).ToArray();
                count += keys.Length;
                if (!keys.Any()) break;
                await _redisDatabase.KeyDeleteAsync(keys);
            }

            await _log.WriteInfoAsync(nameof(AssetsForClientCacheManager), nameof(ClearCache), $"Clear assets cache, count of record: {count}, reason: {reason}");
        }

        public async Task RemoveClientFromChache(string clientId)
        {
            try
            {
                await _cache.RemoveAsync(GetKeyAvailableAssets(clientId, true));
                await _cache.RemoveAsync(GetKeyAvailableAssets(clientId, false));
                await _cache.RemoveAsync(GetKeyCashInViaBankCardEnabled(clientId, false));
                await _cache.RemoveAsync(GetKeySwiftDepositEnabled(clientId, false));
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(RemoveClientFromChache), clientId, ex);
            }
        }

        public async Task SaveAssetForClient(string clientId, bool isIosDevice, IEnumerable<string> clientAssetIds)
        {
            try
            {
                var key = GetKeyAvailableAssets(clientId, isIosDevice);
                var text = clientAssetIds.ToList().ToJson();
                await _cache.SetStringAsync(key, text, new DistributedCacheEntryOptions()
                {
                    AbsoluteExpirationRelativeToNow = _settings.AssetsForClientCacheTimeSpan
                });
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(SaveAssetForClient), clientId, ex);
            }
        }

        public async Task SaveCashInViaBankCardEnabledForClient(string clientId, bool isIosDevice, bool cashInViaBankCardEnabled)
        {
            try
            {
                await _cache.SetStringAsync(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice), cashInViaBankCardEnabled.ToJson(),
                    new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = _settings.AssetsForClientCacheTimeSpan });
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(SaveCashInViaBankCardEnabledForClient), clientId, ex);
            }
        }

        public async Task SaveSwiftDepositEnabledForClient(string clientId, bool isIosDevice, bool swiftDepositEnabled)
        {
            try
            {
                await _cache.SetStringAsync(GetKeySwiftDepositEnabled(clientId, isIosDevice), swiftDepositEnabled.ToJson(),
                    new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = _settings.AssetsForClientCacheTimeSpan });
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(SaveCashInViaBankCardEnabledForClient), clientId, ex);
            }
        }

        public async Task<IReadOnlyList<string>> TryGetAssetForClient(string clientId, bool isIosDevice)
        {
            try
            {
                var json = await _cache.GetStringAsync(GetKeyAvailableAssets(clientId, isIosDevice));
                if (!string.IsNullOrEmpty(json))
                {
                    return json.DeserializeJson<List<string>>();
                }
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(SaveCashInViaBankCardEnabledForClient), clientId, ex);
            }
            return null;
        }

        public async Task<bool?> TryGetSaveCashInViaBankCardEnabledForClient(string clientId, bool isIosDevice)
        {
            try
            {
                var json = await _cache.GetStringAsync(GetKeyCashInViaBankCardEnabled(clientId, isIosDevice));
                if (!string.IsNullOrEmpty(json))
                {
                    return json.DeserializeJson<bool>();
                }
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(SaveCashInViaBankCardEnabledForClient), clientId, ex);
            }
            return null;
        }

        public async Task<bool?> TryGetSaveSwiftDepositEnabledForClient(string clientId, bool isIosDevice)
        {
            try
            {
                var json = await _cache.GetStringAsync(GetKeySwiftDepositEnabled(clientId, isIosDevice));
                if (!string.IsNullOrEmpty(json))
                {
                    return json.DeserializeJson<bool>();
                }
            }
            catch (Exception ex)
            {
                await _log.WriteErrorAsync(nameof(AssetsForClientCacheManager), nameof(SaveCashInViaBankCardEnabledForClient), clientId, ex);
            }
            return null;
        }

        private string GetKeyAvailableAssets(string clientId, bool isIosDevice)
        {
            return string.Format(":Assets:Client:Available_Assets:{0}_{1}", clientId, isIosDevice ? "ios_device" : "");
        }

        private string GetKeyCashInViaBankCardEnabled(string clientId, bool isIosDevice)
        {
            return string.Format(":Assets:Client:CashInViaBankCard:{0}_{1}", clientId, isIosDevice ? "ios_device" : "");
        }

        private string GetKeySwiftDepositEnabled(string clientId, bool isIosDevice)
        {
            return string.Format(":Assets:Client:SwiftDeposit:{0}_{1}", clientId, isIosDevice ? "ios_device" : "");
        }
    }
}
