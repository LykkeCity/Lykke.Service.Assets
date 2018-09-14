using System;
using System.Threading.Tasks;
using AutoMapper;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Cqrs;
using Lykke.Service.Assets.Contract.Events;
using Lykke.Service.Assets.Core.Repositories;

namespace Lykke.Service.Assets.Workflow.Handlers
{
    [UsedImplicitly]
    public class AssetPairHandler
    {
        private readonly IChaosKitty _chaosKitty;
        private readonly IAssetPairRepository _assetPairRepository;

        public AssetPairHandler(
            [NotNull] IChaosKitty chaosKitty,
            [NotNull] IAssetPairRepository assetPairRepository)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _assetPairRepository = assetPairRepository ?? throw new ArgumentNullException(nameof(assetPairRepository));
        }

        public async Task<CommandHandlingResult> Handle(Services.Commands.CreateAssetPairCommand command, IEventPublisher eventPublisher)
        {
            await _assetPairRepository.UpsertAsync(command.AssetPair);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(Mapper.Map<AssetPairCreatedEvent>(command.AssetPair));

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Services.Commands.UpdateAssetPairCommand command, IEventPublisher eventPublisher)
        {
            await _assetPairRepository.UpsertAsync(command.AssetPair);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(Mapper.Map<AssetPairUpdatedEvent>(command.AssetPair));

            return CommandHandlingResult.Ok();
        }
    }
}
