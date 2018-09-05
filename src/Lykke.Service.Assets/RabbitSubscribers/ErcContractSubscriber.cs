using Autofac;
using Common;
using Common.Log;
using JetBrains.Annotations;
using Lykke.Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Assets.Cache;
using Lykke.Service.Assets.Services.Domain;
using System;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.RabbitSubscribers
{
    [UsedImplicitly]
    public class ErcContractSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly ILogFactory _logFactory;
        private readonly string _connectionString;
        private RabbitMqSubscriber<Erc20ContractCreatedMessage> _subscriber;
        private readonly ICachedErc20TokenService _erc20TokenService;

        public ErcContractSubscriber(ILogFactory logFactory,
            string connectionString,
            ICachedErc20TokenService erc20TokenService)
        {
            _log = logFactory.CreateLog(this);
            _logFactory = logFactory;
            _connectionString = connectionString;
            _erc20TokenService = erc20TokenService;
        }

        public void Start()
        {
            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_connectionString, "ethereum.indexer.erccontracts", "service.assets")
                .MakeDurable();

            _subscriber = new RabbitMqSubscriber<Erc20ContractCreatedMessage>(
                    _logFactory,
                    settings,
                    new ResilientErrorHandlingStrategy(_logFactory, settings,
                        retryTimeout: TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_logFactory, settings)))
                .SetMessageDeserializer(new JsonMessageDeserializer<Erc20ContractCreatedMessage>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .Start();
        }

        private async Task ProcessMessageAsync(Erc20ContractCreatedMessage arg)
        {
            _log.Info($"Got Erc20ContractCreatedMessage: {arg.Address} ");

            var token = new Erc20Token
            {
                Address = arg.Address,
                AssetId = null,
                BlockHash = arg.BlockHash,
                BlockTimestamp = arg.BlockTimestamp,
                DeployerAddress = arg.DeployerAddress,
                TokenDecimals = (int?)arg.TokenDecimals,
                TokenName = arg.TokenName,
                TokenSymbol = arg.TokenSymbol,
                TokenTotalSupply = arg.TokenTotalSupply,
                TransactionHash = arg.TransactionHash
            };

            // todo: refactor into event generation + projection logic
            var existingContract = await _erc20TokenService.GetByTokenAddressAsync(token.Address);
            if (existingContract == null)
            {
                await _erc20TokenService.AddAsync(token);
            }
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber?.Stop();
        }
    }
}
