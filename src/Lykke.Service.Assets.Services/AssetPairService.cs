using System;
using System.Collections.Generic;
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
    public class AssetPairService : IAssetPairService, IStartable
    {
        private readonly IAssetPairRepository _assetPairRepository;
        [NotNull] private readonly IMyNoSqlWriterWrapper<AssetPairNoSql> _myNoSqlWriter;
        private readonly ICqrsEngine _cqrsEngine;


        public AssetPairService(
            [NotNull] IAssetPairRepository assetPairRepository,
            [NotNull] IMyNoSqlWriterWrapper<AssetPairNoSql> myNoSqlWriter,
            [NotNull] ICqrsEngine cqrsEngine)
        {
            _assetPairRepository = assetPairRepository ?? throw new ArgumentNullException(nameof(assetPairRepository));
            _myNoSqlWriter = myNoSqlWriter ?? throw new ArgumentNullException(nameof(myNoSqlWriter));
            _cqrsEngine = cqrsEngine ?? throw new ArgumentNullException(nameof(cqrsEngine));
        }


        public async Task<IAssetPair> AddAsync(IAssetPair assetPair)
        {
            await _assetPairRepository.UpsertAsync(assetPair);

            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetPairNoSql.Create(assetPair));

            //todo: remove cqrs
            _cqrsEngine.SendCommand(
                new CreateAssetPairCommand { AssetPair = Mapper.Map<AssetPair>(assetPair) },
                BoundedContext.Name, BoundedContext.Name);

            return assetPair;
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
            await _myNoSqlWriter.TryDeleteAsync(AssetPairNoSql.GeneratePartitionKey(), AssetPairNoSql.GenerateRowKey(id));
        }

        public async Task UpdateAsync(IAssetPair assetPair)
        {
            await _assetPairRepository.UpsertAsync(assetPair);

            await _myNoSqlWriter.TryInsertOrReplaceAsync(AssetPairNoSql.Create(assetPair));

            //todo: remove cqrs
            _cqrsEngine.SendCommand(
                new UpdateAssetPairCommand { AssetPair = Mapper.Map<AssetPair>(assetPair) },
                BoundedContext.Name, BoundedContext.Name);
        }

        public void Start()
        {
            _myNoSqlWriter.Start(ReadAllData);
        }

        private IList<AssetPairNoSql> ReadAllData()
        {
            var data = GetAllAsync().GetAwaiter().GetResult().Select(AssetPairNoSql.Create).ToList();
            return data;
        }
    }
}
