using System;
using System.Threading.Tasks;
using Autofac;
using Common;
using Common.Log;
using Lykke.RabbitMqBroker;
using Lykke.RabbitMqBroker.Subscriber;
using Lykke.Service.Assets.Core.Services;
using Lykke.Job.Asset.IncomingMessages;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Asset.RabbitSubscribers
{
    public class ErcContractSubscriber : IStartable, IStopable
    {
        private readonly ILog _log;
        private readonly string _connectionString;
        private readonly IErcContractProcessor _ercContractProcessor;
        private RabbitMqSubscriber<Erc20ContractCreatedMessage> _subscriber;

        public ErcContractSubscriber(ILog log, IErcContractProcessor ercContractProcessor, string connectionString)
        {
            _log = log;
            _connectionString = connectionString;
            _ercContractProcessor = ercContractProcessor;
        }

        public void Start()
        {
            // NOTE: Read https://github.com/LykkeCity/Lykke.RabbitMqDotNetBroker/blob/master/README.md to learn
            // about RabbitMq subscriber configuration

            var settings = RabbitMqSubscriptionSettings
                .CreateForSubscriber(_connectionString, "lykke.ethereum.indexer", "erccontracts")
                .MakeDurable();
            // TODO: Make additional configuration, using fluent API here:
            // ex: .MakeDurable()

            _subscriber = new RabbitMqSubscriber<Erc20ContractCreatedMessage>(settings,
                    new ResilientErrorHandlingStrategy(_log, settings,
                        retryTimeout: TimeSpan.FromSeconds(10),
                        next: new DeadQueueErrorHandlingStrategy(_log, settings)))
                .SetMessageDeserializer(new JsonMessageDeserializer<Erc20ContractCreatedMessage>())
                .SetMessageReadStrategy(new MessageReadQueueStrategy())
                .Subscribe(ProcessMessageAsync)
                .CreateDefaultBinding()
                .SetLogger(_log)
                .Start();
        }

        private async Task ProcessMessageAsync(Erc20ContractCreatedMessage arg)
        {
            // TODO: Orchestrate execution flow here and delegate actual business logic implementation to services layer
            // Do not implement actual business logic here
            Erc20Asset message = new Erc20Asset()
            {
                Address = arg.Address,
                AssetId = null,
                BlockHash = arg.BlockHash,
                BlockTimestamp = arg.BlockTimestamp,
                DeployerAddress = arg.DeployerAddress,
                TokenDecimals = arg.TokenDecimals,
                TokenName = arg.TokenName,
                TokenSymbol = arg.TokenSymbol,
                TokenTotalSupply = arg.TokenTotalSupply,
                TransactionHash = arg.TransactionHash
            };

            try
            {
                await _ercContractProcessor.ProcessErc20ContractAsync(message);

            } catch (Exception e)
            {
                //TODO: Add retry logic
            }
        }

        public void Dispose()
        {
            _subscriber?.Dispose();
        }

        public void Stop()
        {
            _subscriber.Stop();
        }
    }
}