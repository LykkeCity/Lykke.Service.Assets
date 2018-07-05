using System;
using System.Threading.Tasks;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Services.Events;

namespace Lykke.Service.Assets.Services.Handlers
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

            eventPublisher.PublishEvent(new AssetCreatedEvent { Asset = command.Asset });

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Commands.UpdateAssetCommand command, IEventPublisher eventPublisher)
        {
            await _assetRepository.UpdateAsync(command.Asset);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(new AssetUpdatedEvent { Asset = command.Asset });

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
