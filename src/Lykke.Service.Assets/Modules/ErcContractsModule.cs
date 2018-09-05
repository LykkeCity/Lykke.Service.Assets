using Autofac;
using Lykke.Service.Assets.Settings;
using Lykke.Service.Assets.Workflow.Handlers;
using Lykke.SettingsReader;

namespace Lykke.Service.Assets.Modules
{
    public class ErcContractsModule : Module
    {
        private readonly ApplicationSettings.AssetsSettings _settings;

        public ErcContractsModule(
            IReloadingManager<ApplicationSettings> settings)
        {
            _settings = settings.CurrentValue.AssetsService;
        }

        protected override void Load(ContainerBuilder builder)
        {
            RegisterRabbitMqSubscribers(builder);
        }

        private void RegisterRabbitMqSubscribers(ContainerBuilder builder)
        {
            builder.RegisterType<ErcContractSubscriber>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance()
                .WithParameter(TypedParameter.From(_settings.Rabbit.ConnectionString));
        }
    }
}
