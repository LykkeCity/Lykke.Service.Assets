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
        private readonly ILog _log;
        private readonly IChaosKitty _chaosKitty;
        private readonly IAssetRepository _assetRepository;
        private readonly IAssetPairRepository _assetPairRepository;

        public AssetsHandler(
            [NotNull] ILogFactory logFactory,
            [NotNull] IChaosKitty chaosKitty,
            [NotNull] IAssetRepository assetRepository,
            [NotNull] IAssetPairRepository assetPairRepository)
        {
            if (logFactory == null)
                throw new ArgumentNullException(nameof(logFactory));
            _log = logFactory.CreateLog(this);
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
            _assetPairRepository = assetPairRepository ?? throw new ArgumentNullException(nameof(assetPairRepository));
        }

        public async Task<CommandHandlingResult> Handle(Commands.CreateAssetCommand command, IEventPublisher eventPublisher)
        {
            _log.Info(nameof(Commands.CreateAssetCommand), "Creating asset", command);

            await _assetRepository.InsertOrReplaceAsync(command.Asset);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(new AssetCreatedEvent { Asset = command.Asset });

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Commands.UpdateAssetCommand command, IEventPublisher eventPublisher)
        {
            _log.Info(nameof(Commands.UpdateAssetCommand), "Updating asset", command);

            await _assetRepository.UpdateAsync(command.Asset);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(new AssetUpdatedEvent { Asset = command.Asset });

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Commands.CreateAssetPairCommand command, IEventPublisher eventPublisher)
        {
            _log.Info(nameof(Commands.CreateAssetPairCommand), "Creating asset pair", command);

            await _assetPairRepository.UpsertAsync(command.AssetPair);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(new AssetPairCreatedEvent { AssetPair = command.AssetPair });

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Commands.UpdateAssetPairCommand command, IEventPublisher eventPublisher)
        {
            _log.Info(nameof(Commands.UpdateAssetPairCommand), "Updating asset pair", command);

            await _assetPairRepository.UpsertAsync(command.AssetPair);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(new AssetPairUpdatedEvent { AssetPair = command.AssetPair });

            return CommandHandlingResult.Ok();
        }
    }
}
