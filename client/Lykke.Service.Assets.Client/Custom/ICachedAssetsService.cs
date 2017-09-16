﻿using Lykke.Service.Assets.Client.Models;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Client.Custom
{
    /// <summary>
    /// Provides assets data, cached on the client's side
    /// </summary>
    public interface ICachedAssetsService
    {
        Task<IAssetPair> TryGetAssetPairAsync(string assetPairId, CancellationToken cancellationToken = new CancellationToken());
        Task<IReadOnlyCollection<IAssetPair>> GetAllAssetPairsAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<IAsset> TryGetAssetAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());
        Task<IReadOnlyCollection<IAsset>> GetAllAssetsAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<IAssetAttributes> GetAssetAttributesAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());
        Task<IAssetAttributes> GetAssetAttributeByKeyAsync(string assetId, string key, CancellationToken cancellationToken = new CancellationToken());
        Task<IAssetDescription> GetAssetDescriptionsAsync(GetAssetDescriptionsRequestModel ids, CancellationToken cancellationToken = new CancellationToken());
        Task<IReadOnlyCollection<IAssetCategory>> GetAssetCategoriesAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<AssetCategoriesResponseModel> TryGetAssetCategoryAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());
        Task<AssetExtendedResponseModel> GetAssetsExtendedAsync(CancellationToken cancellationToken = new CancellationToken());
        Task<AssetExtendedResponseModel> GetAssetExtendedByIdAsync(string assetId, CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Forcibly updates server-side and client-side asset pairs cache
        /// </summary>
        Task UpdateAssetPairsCacheAsync(CancellationToken cancellationToken = new CancellationToken());

        /// <summary>
        /// Forcibly updates server-side and client-side assets cache
        /// </summary>
        Task UpdateAssetsCacheAsync(CancellationToken cancellationToken = new CancellationToken());
    }
}