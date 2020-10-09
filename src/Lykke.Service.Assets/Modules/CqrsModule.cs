using System.Collections.Generic;
using Autofac;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
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

        public CqrsModule(IReloadingManager<ApplicationSettings.AssetsSettings> settingsManager)
        {
            _settings = settingsManager.CurrentValue;
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

            var rabbitMqSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.Cqrs.RabbitConnectionString };

            builder.RegisterType<AssetsHandler>();

            builder.Register(ctx =>
            {
                const string defaultPipeline = "commands";
                const string defaultRoute = "self";

                return new CqrsEngine(ctx.Resolve<ILogFactory>(),
                    ctx.Resolve<IDependencyResolver>(),

                    new MessagingEngine(ctx.Resolve<ILogFactory>(),
                        new TransportResolver(new Dictionary<string, TransportInfo>
                        {
                            {"RabbitMq", new TransportInfo(rabbitMqSettings.Endpoint.ToString(), rabbitMqSettings.UserName, rabbitMqSettings.Password, "None", "RabbitMq")}
                        }),
                        new RabbitMqTransportFactory(ctx.Resolve<ILogFactory>())),

                    new DefaultEndpointProvider(),
                    true,
                    Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                        "RabbitMq",
                        "messagepack",
                        environment: "lykke",
                        exclusiveQueuePostfix: "k8s")),

                Register.BoundedContext("assets")
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
