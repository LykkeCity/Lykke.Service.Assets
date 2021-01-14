using AutoMapper;
using JetBrains.Annotations;
using Lykke.Common.Chaos;
using Lykke.Cqrs;
using Lykke.Service.Assets.Core.Repositories;
using System;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client.Events;

namespace Lykke.Service.Assets.Workflow.Handlers
{
    [UsedImplicitly]
    public class AssetsHandler
    {
        private readonly IChaosKitty _chaosKitty;
        private readonly IAssetRepository _assetRepository;

        public AssetsHandler(
            [NotNull] IChaosKitty chaosKitty,
            [NotNull] IAssetRepository assetRepository)
        {
            _chaosKitty = chaosKitty ?? throw new ArgumentNullException(nameof(chaosKitty));
            _assetRepository = assetRepository ?? throw new ArgumentNullException(nameof(assetRepository));
        }

        public async Task<CommandHandlingResult> Handle(Services.Commands.CreateAssetCommand command, IEventPublisher eventPublisher)
        {
            //await _assetRepository.InsertOrReplaceAsync(command.Asset);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(Mapper.Map<AssetCreatedEvent>(command.Asset));

            return CommandHandlingResult.Ok();
        }

        public async Task<CommandHandlingResult> Handle(Services.Commands.UpdateAssetCommand command, IEventPublisher eventPublisher)
        {
            //await _assetRepository.UpdateAsync(command.Asset);

            _chaosKitty.Meow("repository unavailable");

            eventPublisher.PublishEvent(Mapper.Map<AssetUpdatedEvent>(command.Asset));

            return CommandHandlingResult.Ok();
        }
    }
}
