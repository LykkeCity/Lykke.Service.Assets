using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Autofac;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Cqrs;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.NoSql.Models;
using Lykke.Service.Assets.Services.Commands;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetService : IAssetService, IStartable
    {
        private readonly IAssetRepository _assetRepository;
        private readonly IMyNoSqlWriterWrapper<AssetNoSql> _myNoSqlWriter;
        private readonly ICqrsEngine _cqrsEngine;


        public AssetService(
            [NotNull] IAssetRepository assetRepository,
            [NotNull] IMyNoSqlWriterWrapper<AssetNoSql> myNoSqlWriter,
            [NotNull] ICqrsEngine cqrsEngine)
        {
            _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
            _myNoSqlWriter = myNoSqlWriter;
            _cqrsEngine = cqrsEngine ?? throw new ArgumentNullException(nameof(cqrsEngine));
        }


        public async Task<IAsset> AddAsync(IAsset asset)
        {
            await ValidateAsset(asset);

            await _assetRepository.InsertOrReplaceAsync(asset);

            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetNoSql.Create(asset));

            
            //todo: remove cqrs
            _cqrsEngine.SendCommand(
                new CreateAssetCommand { Asset = Mapper.Map<Asset>(asset) },
                BoundedContext.Name, BoundedContext.Name);

            return asset;
        }

        public IAsset CreateDefault()
        {
            return new Asset();
        }

        public async Task DisableAsync(string id)
        {
            var asset = Mapper.Map<Asset>(await _assetRepository.GetAsync(id));

            asset.IsDisabled = true;

            await UpdateAsync(asset);
        }

        public async Task EnableAsync(string id)
        {
            var asset = Mapper.Map<Asset>(await _assetRepository.GetAsync(id));

            asset.IsDisabled = false;

            await UpdateAsync(asset);
        }

        public async Task<IEnumerable<IAsset>> GetAllAsync(bool includeNonTradable)
        {
            return await _assetRepository.GetAllAsync(includeNonTradable);
        }

        public async Task<IEnumerable<IAsset>> GetAsync(string[] ids, bool? isTradable)
        {
            return await _assetRepository.GetAsync(ids, isTradable);
        }

        public async Task<IAsset> GetAsync(string id)
        {
            return await _assetRepository.GetAsync(id);
        }
        
        public async Task RemoveAsync(string id)
        {
            await _assetRepository.RemoveAsync(id);
            await _myNoSqlWriter.TryDeleteAsync(AssetNoSql.GeneratePartitionKey(), AssetNoSql.GenerateRowKey(id));
        }

        public async Task UpdateAsync(IAsset asset)
        {
            await ValidateAsset(asset);

            await _assetRepository.UpdateAsync(asset);

            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetNoSql.Create(asset));


            //todo: remove cqrs
            _cqrsEngine.SendCommand(
                new UpdateAssetCommand { Asset = Mapper.Map<Asset>(asset) },
                BoundedContext.Name, BoundedContext.Name);
        }

        private async Task ValidateAsset(IAsset asset)
        {
            ValidateAccuracyAndMultiplierPower(asset);

            await ValidateBlockchainAssetId(asset);
        }

        private static void ValidateAccuracyAndMultiplierPower(IAsset asset)
        {
            if (asset.Accuracy > asset.MultiplierPower)
            {
                throw new ValidationException($"Asset accuracy [{asset.Accuracy}] should be less or equal to multiplier power [{asset.MultiplierPower}].");
            }
        }

        private async Task ValidateBlockchainAssetId(IAsset asset)
        {
            if (!string.IsNullOrEmpty(asset.BlockChainAssetId))
            {
                var assets = await _assetRepository.GetAllAsync(true);

                if (assets.Any(x => x.BlockChainAssetId == asset.BlockChainAssetId && x.Id != asset.Id))
                {
                    throw new ValidationException($"Another asset [{asset.Id}] with specified BlockChainAssetId [{asset.BlockChainAssetId}] already exists");
                }
            }
        }

        public void Start()
        {
            _myNoSqlWriter.Start(GetAllData);
        }

        private IList<AssetNoSql> GetAllData()
        {
            var data = GetAllAsync(true).GetAwaiter().GetResult().Select(AssetNoSql.Create).ToList();
            return data;
        }
    }
}
