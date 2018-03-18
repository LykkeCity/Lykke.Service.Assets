using System.Collections.Generic;
using Autofac;
using Common.Log;
using Lykke.Common.Chaos;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Services.Commands;
using Lykke.Service.Assets.Services.Events;
using Lykke.Service.Assets.Services.Handlers;
using Lykke.SettingsReader;

namespace Lykke.Service.Assets.Modules
{
    public class CqrsModule : Module
    {
        private readonly ApplicationSettings.AssetsSettings _settings;
        private readonly ILog _log;

        public CqrsModule(IReloadingManager<ApplicationSettings.AssetsSettings> settingsManager, ILog log)
        {
            _settings = settingsManager.CurrentValue;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            if (_settings.ChaosKitty != null)
            {
                builder
                    .RegisterType<ChaosKitty>()
                    .WithParameter(TypedParameter.From(_settings.ChaosKitty.StateOfChaos))
                    .As<IChaosKitty>()
                    .SingleInstance();
            }
            else
            {
                builder
                    .RegisterType<SilentChaosKitty>()
                    .As<IChaosKitty>()
                    .SingleInstance();
            }

            Messaging.Serialization.MessagePackSerializerFactory.Defaults.FormatterResolver = MessagePack.Resolvers.ContractlessStandardResolver.Instance;

            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>().SingleInstance();

            var rabbitMqSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.SagasRabbitMqConnStr };
#if DEBUG
            var virtualHost = "/debug";
            var messagingEngine = new MessagingEngine(_log,
                new TransportResolver(new Dictionary<string, TransportInfo>
                {
                    {"RabbitMq", new TransportInfo(rabbitMqSettings.Endpoint + virtualHost, rabbitMqSettings.UserName, rabbitMqSettings.Password, "None", "RabbitMq")}
                }),
                new RabbitMqTransportFactory());
#else
            var messagingEngine = new MessagingEngine(_log,
                new TransportResolver(new Dictionary<string, TransportInfo>
                {
                    {"RabbitMq", new TransportInfo(rabbitMqSettings.Endpoint.ToString(), rabbitMqSettings.UserName, rabbitMqSettings.Password, "None", "RabbitMq")}
                }),
                new RabbitMqTransportFactory());
#endif

            var defaultRetryDelay = (long)_settings.RetryDelay.TotalMilliseconds;

            builder.RegisterType<AssetsHandler>();

            builder.Register(ctx =>
            {
                const string defaultPipeline = "commands";
                const string defaultRoute = "self";

                return new CqrsEngine(_log,
                    ctx.Resolve<IDependencyResolver>(),
                    messagingEngine,
                    new DefaultEndpointProvider(),
                    true,
                    Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                        "RabbitMq",
                        "messagepack",
                        environment: "lykke",
                        exclusiveQueuePostfix: _settings.QueuePostfix)),

                Register.BoundedContext("assets")
                    .FailedCommandRetryDelay(defaultRetryDelay)
                    .ListeningCommands(
                            typeof(CreateAssetCommand),
                            typeof(UpdateAssetCommand),
                            typeof(CreateAssetPairCommand),
                            typeof(UpdateAssetPairCommand))
                        .On(defaultRoute)
                    .PublishingEvents(
                            typeof(AssetCreatedEvent),
                            typeof(AssetUpdatedEvent),
                            typeof(AssetPairCreatedEvent),
                            typeof(AssetPairUpdatedEvent))
                        .With(defaultPipeline)
                    .WithCommandsHandler<AssetsHandler>(),

                Register.DefaultRouting
                    .PublishingCommands(
                            typeof(CreateAssetCommand),
                            typeof(UpdateAssetCommand),
                            typeof(CreateAssetPairCommand),
                            typeof(UpdateAssetPairCommand))
                        .To("assets").With(defaultPipeline)
                );
            })
            .As<ICqrsEngine>().SingleInstance();
        }
    }
}
