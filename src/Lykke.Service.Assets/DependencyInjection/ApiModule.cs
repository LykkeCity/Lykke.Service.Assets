using System;
using Autofac;
using AzureStorage.Tables;
using Common.Log;
using Lykke.Service.Assets.Core;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Repositories;
using Lykke.Service.Assets.Repositories.Entities;
using Lykke.Service.Assets.Services;
using AzureStorage.Tables.Templates.Index;
using Lykke.Service.Assets.RabbitSubscribers;
using Lykke.SettingsReader;

namespace Lykke.Service.Assets.DependencyInjection
{
    public class ApiModule : Module
    {
        private readonly IReloadingManager<ApplicationSettings> _settings;
        private readonly ILog _log;

        public ApiModule(IReloadingManager<ApplicationSettings> settings, ILog log)
        {
            _settings = settings;
            _log = log;
        }

        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterInstance(_log).SingleInstance();

            builder.RegisterInstance(_settings).SingleInstance();
            builder.RegisterInstance(_settings.CurrentValue.AssetsService).SingleInstance();
            builder.RegisterModule(new RepositoriesModule(_log, _settings));
            builder.RegisterModule(new ServicesModule());
            RegisterRabbitMqSubscribers(builder);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            // TODO: You should register each subscriber in DI container as IStartable singleton and autoactivate it

            builder.RegisterType<ErcContractSubscriber>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.CurrentValue.AssetsService.Rabbit.ConnectionString));
        }
    }
}