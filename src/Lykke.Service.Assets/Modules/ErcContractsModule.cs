using Autofac;
using Lykke.Service.Assets.RabbitSubscribers;
using Lykke.Service.Assets.Settings;
using Lykke.SettingsReader;

namespace Lykke.Service.Assets.Modules
{
    public class ErcContractsModule : Module
    {
        private readonly IReloadingManager<ApplicationSettings> _settings;

        public ErcContractsModule(
            IReloadingManager<ApplicationSettings> settings)
        {
            _settings = settings;
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
                .WithParameter(TypedParameter.From(_settings.CurrentValue.AssetsService.Rabbit.ConnectionString));
        }
    }
}
