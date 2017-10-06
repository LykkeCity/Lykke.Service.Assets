using Autofac;
using Autofac.Extensions.DependencyInjection;
using Common.Log;
using Lykke.SettingsReader;
using Lykke.JobTriggers.Extenstions;
using Lykke.Job.Asset.RabbitSubscribers;
using Lykke.RabbitMq.Azure;
using Lykke.RabbitMqBroker.Publisher;
using AzureStorage.Blob;
using Microsoft.Extensions.DependencyInjection;
using Lykke.Job.Asset.Services;
using Lykke.Job.Asset.Core.Services;
using Lykke.Service.Assets.Core;
using Lykke.Job.Asset.Core.Repositories;
using Lykke.Service.Assets.Repositories;
using AzureStorage.Tables;
using Lykke.Job.Asset.Core.Domain;
using Lykke.Service.Assets.Repositories.Entities;
using AzureStorage.Tables.Templates.Index;

namespace Lykke.Job.Asset.Modules
{
    public class JobModule : Module
    {
        private readonly ApplicationSettings _settings;
        private readonly IReloadingManager<ApplicationSettings> _dbSettingsManager;
        private readonly ILog _log;
        // NOTE: you can remove it if you don't need to use IServiceCollection extensions to register service specific dependencies
        private readonly IServiceCollection _services;

        public JobModule(ApplicationSettings settings, IReloadingManager<ApplicationSettings> dbSettingsManager, ILog log)
        {
            _settings = settings;
            _log = log;
            _dbSettingsManager = dbSettingsManager;

            _services = new ServiceCollection();
        }

        protected override void Load(ContainerBuilder builder)
        {
            // NOTE: Do not register entire settings in container, pass necessary settings to services which requires them
            // ex:
            // builder.RegisterType<QuotesPublisher>()
            //  .As<IQuotesPublisher>()
            //  .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString))

            builder.RegisterInstance(_log)
                .As<ILog>()
                .SingleInstance();

            builder.RegisterType<HealthService>()
                .As<IHealthService>()
                .SingleInstance();

            builder.RegisterType<StartupManager>()
                .As<IStartupManager>();

            builder.RegisterType<ShutdownManager>()
                .As<IShutdownManager>();

            RegisterServicesForJobs(builder);
            RegisterRepositoriesForJobs(builder);
            RegisterRabbitMqSubscribers(builder);

            builder.Populate(_services);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            // TODO: You should register each subscriber in DI container as IStartable singleton and autoactivate it

            builder.RegisterType<ErcContractSubscriber>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.AssetsService.Rabbit.ConnectionString));
        }

        private void RegisterServicesForJobs(ContainerBuilder builder)
        {
            builder.RegisterType<ErcContractProcessor>()
                .As<IErcContractProcessor>().SingleInstance();
        }

        private void RegisterRepositoriesForJobs(ContainerBuilder builder)
        {
            builder.RegisterInstance<IErc20AssetRepository>(
                new Erc20AssetRepository(AzureTableStorage<Erc20AssetEntity>
                .Create(_dbSettingsManager.Nested(x => x.AssetsService.Dictionaries.DbConnectionString),
                    "Erc20Asset", _log),
                 AzureTableStorage<AzureIndex>
                .Create(_dbSettingsManager.Nested(x => x.AssetsService.Dictionaries.DbConnectionString),
                    "Erc20Asset", _log)));
        }
    }
}