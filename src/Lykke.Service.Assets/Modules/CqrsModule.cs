using System.Collections.Generic;
using Autofac;
using Lykke.Common.Chaos;
using Lykke.Common.Log;
using Lykke.Cqrs;
using Lykke.Cqrs.Configuration;
using Lykke.Messaging;
using Lykke.Messaging.RabbitMq;
using Lykke.Messaging.Serialization;
using Lykke.Service.Assets.Contract.Events;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Services;
using Lykke.Service.Assets.Services.Commands;
using Lykke.Service.Assets.Services.Events;
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
            
            builder.Register(context => new AutofacDependencyResolver(context)).As<IDependencyResolver>().SingleInstance();

            var rabbitMqSettings = new RabbitMQ.Client.ConnectionFactory { Uri = _settings.SagasRabbitMqConnStr };

#if DEBUG
            const string virtualHost = "/debug";
#endif

            var defaultRetryDelay = (long)_settings.RetryDelay.TotalMilliseconds;

            builder.RegisterType<AssetsHandler>();

            builder.Register(ctx =>
            {
                const string defaultPipeline = "commands";
                const string defaultRoute = "self";

                return new CqrsEngine(ctx.Resolve<ILogFactory>(),
                    ctx.Resolve<IDependencyResolver>(),
#if DEBUG
                    new MessagingEngine(ctx.Resolve<ILogFactory>(),
                        new TransportResolver(new Dictionary<string, TransportInfo>
                        {
                            {"RabbitMq", new TransportInfo(rabbitMqSettings.Endpoint + virtualHost, rabbitMqSettings.UserName, rabbitMqSettings.Password, "None", "RabbitMq")}
                        }),
                        new RabbitMqTransportFactory(ctx.Resolve<ILogFactory>())),
#else
                    new MessagingEngine(ctx.Resolve<ILogFactory>(),
                        new TransportResolver(new Dictionary<string, TransportInfo>
                        {
                            {"RabbitMq", new TransportInfo(rabbitMqSettings.Endpoint.ToString(), rabbitMqSettings.UserName, rabbitMqSettings.Password, "None", "RabbitMq")}
                        }),
                        new RabbitMqTransportFactory(ctx.Resolve<ILogFactory>())),
#endif
                    new DefaultEndpointProvider(),
                    true,
                    Register.DefaultEndpointResolver(new RabbitMqConventionEndpointResolver(
                        "RabbitMq",
                        SerializationFormat.ProtoBuf,
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
