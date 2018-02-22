using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Cqrs;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Commands;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AssetPairService : IAssetPairService
    {
        private readonly IAssetPairRepository _assetPairRepository;
        private readonly ICqrsEngine _cqrsEngine;


        public AssetPairService(
            [NotNull] IAssetPairRepository assetPairRepository,
            [NotNull] ICqrsEngine cqrsEngine)
        {
            _assetPairRepository = assetPairRepository ?? throw new ArgumentNullException(nameof(assetPairRepository));
            _cqrsEngine = cqrsEngine ?? throw new ArgumentNullException(nameof(cqrsEngine));
        }


        public Task<IAssetPair> AddAsync(IAssetPair assetPair)
        {
            _cqrsEngine.SendCommand(
                new CreateAssetPairCommand { AssetPair = Mapper.Map<AssetPair>(assetPair) },
                "assets", "assets");

            return Task.FromResult(assetPair);
        }

        public IAssetPair CreateDefault()
        {
            return new AssetPair
            {
                Accuracy = 5,
                IsDisabled = false
            };
        }

        public async Task<IEnumerable<IAssetPair>> GetAllAsync()
        {
            return await _assetPairRepository.GetAllAsync();
        }

        public async Task<IAssetPair> GetAsync(string id)
        {
            return await _assetPairRepository.GetAsync(id);
        }

        public async Task RemoveAsync(string id)
        {
            await _assetPairRepository.RemoveAsync(id);
        }

        public Task UpdateAsync(IAssetPair assetPair)
        {
            _cqrsEngine.SendCommand(
                new UpdateAssetPairCommand { AssetPair = Mapper.Map<AssetPair>(assetPair) },
                "assets", "assets");

            return Task.CompletedTask;
        }
    }
}
