﻿using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Models;

namespace Lykke.Service.Assets.Client.Custom
{
    public class CachedAssetsService : ICachedAssetsService
    {
        private readonly IAssetsservice _assetsservice;
        private readonly IDictionaryCache<AssetResponseModel> _assetsCache;
        private readonly IDictionaryCache<AssetPairResponseModel> _assetPairsCache;
        private readonly IDictionaryCache<AssetCategoriesResponseModel> _assetCategoriesCache;

        public CachedAssetsService(IAssetsservice assetsservice, 
            IDictionaryCache<AssetResponseModel> assetsCache, 
            IDictionaryCache<AssetPairResponseModel> assetPairsCache,
            IDictionaryCache<AssetCategoriesResponseModel> assetCategoriesCache)
        {
            _assetsservice = assetsservice;
            _assetsCache = assetsCache;
            _assetPairsCache = assetPairsCache;
            _assetCategoriesCache = assetCategoriesCache;
        }

        public async Task<IAssetAttributes> GetAssetAttributesAsync(string assetId, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _assetsservice.GetAssetAttributesAsync(assetId, cancellationToken);
        }

        public async Task<IAssetAttributes> GetAssetAttributeByKeyAsync(string assetId, string key, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _assetsservice.GetAssetAttributeByKeyAsync(assetId, key, cancellationToken);
        }

        public async Task<IAssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetPairsAsync(cancellationToken));

            return _assetPairsCache.TryGet(assetPairId);
        }

        public async Task<IReadOnlyCollection<IAssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetPairsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetPairsAsync(cancellationToken));

            return _assetPairsCache.GetAll();
        }

        public async Task<IAsset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetsAsync(cancellationToken));
            
            return _assetsCache.TryGet(assetId);
        }

        public async Task<IReadOnlyCollection<IAsset>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetsAsync(cancellationToken));

            return _assetsCache.GetAll();
        }

        public async Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsservice.UpdateAssetPairsCacheAsync(cancellationToken);

            _assetPairsCache.Update(await GetUncachedAssetPairsAsync(cancellationToken));
        }

        public async Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken())
        {
            await _assetsservice.UpdateAssetsCacheAsync(cancellationToken);

            _assetsCache.Update(await GetUncachedAssetsAsync(cancellationToken));
        }

        private async Task<IEnumerable<AssetResponseModel>> GetUncachedAssetsAsync(CancellationToken cancellationToken)
        {
            return await _assetsservice.GetAssetsAsync(cancellationToken);
        }

        private async Task<IEnumerable<AssetPairResponseModel>> GetUncachedAssetPairsAsync(CancellationToken cancellationToken)
        {
            return await _assetsservice.GetAssetPairsAsync(cancellationToken);
        }

        public async Task<IAssetExtendedInfo> GetAssetDescriptionsAsync(GetAssetDescriptionsRequestModel ids, CancellationToken cancellationToken = default(CancellationToken))
        {
            return await _assetsservice.GetAssetDescriptionsAsync(ids, cancellationToken);
        }

        public async Task<IReadOnlyCollection<IAssetCategory>> GetAssetCategoriesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            await _assetCategoriesCache.EnsureCacheIsUpdatedAsync(() => GetUncachedAssetCategoriesAsync(cancellationToken));

            return _assetCategoriesCache.GetAll();
        }

        public async Task<AssetCategoriesResponseModel> TryGetAssetCategoryAsync(string assetId, CancellationToken cancellationToken = default(CancellationToken))
        {
            var res = await _assetsservice.GetAssetCategoryAsync(assetId);
            if (res is ErrorResponse)
                return AssetCategoriesResponseModel.Create((ErrorResponse)res);
            if (res is IAssetCategory)
                return AssetCategoriesResponseModel.Create(res as IAssetCategory);
            else
                return new AssetCategoriesResponseModel();
        }

        private async Task<IEnumerable<AssetCategoriesResponseModel>> GetUncachedAssetCategoriesAsync(CancellationToken cancellationToken)
        {
            return await _assetsservice.GetAssetCategoriesAsync(cancellationToken);
        }


    }
}