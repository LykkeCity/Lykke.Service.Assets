using System;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Cqrs;
using Lykke.Service.Assets.Contract.Events;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Services.Events;

namespace Lykke.Service.Assets.Services
{
    // todo: split into 'assets' + 'asset-pairs' handles
    [UsedImplicitly]
    public class AssetsHandler
    {
        private readonly IChaosKitty _chaosKitty;
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetPairRepository _assetPairRepository;

        public AssetsHandler(
            [NotNull] IChaosKitty chaosKitty,
            [NotNull] IAssetRepository assetRepository,
            [NotNull] IAssetPairRepository assetPairRepository)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
            _assetPairRepository = assetPairRepository ?? throw new ArgumentNullException(nameof(assetPairRepository));
        }

        public async Task<CommandHandlingResult> Handle(Commands.CreateAssetCommand command, IEventPublisher eventPublisher)
        {
            await _assetRepository.InsertOrReplaceAsync(command.Asset);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(Mapper.Map<AssetCreatedEvent>(command.Asset));

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Commands.UpdateAssetCommand command, IEventPublisher eventPublisher)
        {
            await _assetRepository.UpdateAsync(command.Asset);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(Mapper.Map<AssetUpdatedEvent>(command.Asset));

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Commands.CreateAssetPairCommand command, IEventPublisher eventPublisher)
        {
            await _assetPairRepository.UpsertAsync(command.AssetPair);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(new AssetPairCreatedEvent { AssetPair = command.AssetPair });

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Commands.UpdateAssetPairCommand command, IEventPublisher eventPublisher)
        {
            await _assetPairRepository.UpsertAsync(command.AssetPair);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(new AssetPairUpdatedEvent { AssetPair = command.AssetPair });

            return CommandHandlingResult.Ok();
        }
    }
}
