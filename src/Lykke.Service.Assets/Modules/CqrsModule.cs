using System;
using Autofac;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Messaging.Serialization;
using Lykke.Service.Assets.Services.Commands;
using Lykke.Service.Assets.Settings;
using Lykke.Service.Assets.Workflow.Handlers;
using Lykke.SettingsReader;
using System.Collections.Generic;
using Lykke.Messaging.Contract;
using Lykke.Service.Assets.Client.Events;

namespace Lykke.Service.Assets.Modules
{
    public class CqrsModule : Module
    {
        private readonly CqrsSettings _settings;

        public CqrsModule(IReloadingManager<ApplicationSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue.AssetsService.Cqrs;
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

            MessagePackSerializerFactory.Defaults.FormatterResolver = MessagePack.Resolvers.ContractlessStandardResolver.Instance;

            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>().SingleInstance();

            var rabbitMqSettings = new RabbitMQ.Client.ConnectionFactory { Uri = new Uri(_settings.RabbitConnectionString) };

            builder.RegisterType<AssetsHandler>();
            builder.RegisterType<AssetPairHandler>();

            builder.Register(ctx => new MessagingEngine(
                    ctx.Resolve<ILogFactory>(),
                    new TransportResolver(new Dictionary<string, TransportInfo>
                    {
                        {
                            "RabbitMq",
                            new TransportInfo(rabbitMqSettings.Endpoint.ToString(), rabbitMqSettings.UserName,
                                rabbitMqSettings.Password, "None", "RabbitMq")
                        }
                    }),
                    new RabbitMqTransportFactory(ctx.Resolve<ILogFactory>())))
                .As<IMessagingEngine>().SingleInstance();

            builder.Register(ctx =>
            {
                const string defaultPipeline = "commands";
                const string defaultRoute = "self";

                var engine = new CqrsEngine(
                    ctx.Resolve<ILogFactory>(),
                    ctx.Resolve<IDependencyResolver>(),
                    ctx.Resolve<IMessagingEngine>(),
                    new DefaultEndpointProvider(),
                    true,
                    Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                        "RabbitMq",
                        SerializationFormat.MessagePack,
                        environment: "lykke",
                        exclusiveQueuePostfix: "k8s")),
                    Register.BoundedContext(BoundedContext.Name)
                        .ListeningCommands(
                                typeof(CreateAssetCommand),
                                typeof(UpdateAssetCommand))
                            .On(defaultRoute)
                        .PublishingEvents(
                                typeof(AssetCreatedEvent),
                                typeof(AssetUpdatedEvent))
                            .With(defaultPipeline)
                        .WithCommandsHandler<AssetsHandler>()
                        .ListeningCommands(
                                typeof(CreateAssetPairCommand),
                                typeof(UpdateAssetPairCommand))
                            .On(defaultRoute)
                        .PublishingEvents(
                                typeof(AssetPairCreatedEvent),
                                typeof(AssetPairUpdatedEvent))
                            .With(defaultPipeline)
                        .WithCommandsHandler<AssetPairHandler>(),
                    Register.DefaultRouting
                        .PublishingCommands(
                                typeof(CreateAssetCommand),
                                typeof(UpdateAssetCommand),
                                typeof(CreateAssetPairCommand),
                                typeof(UpdateAssetPairCommand))
                            .To(BoundedContext.Name).With(defaultPipeline)
                    );
                engine.StartPublishers();
                return engine;
            })
            .As<ICqrsEngine>()
            .SingleInstance()
            .AutoActivate();
        }
    }
}
