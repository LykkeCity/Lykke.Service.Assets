using Autofac;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<AssetAttributeService>()
                .As<IAssetAttributeService>()
                .SingleInstance();

            builder
                .RegisterType<AssetCategoryService>()
                .As<IAssetCategoryService>()
                .SingleInstance();

            builder.RegisterType<Erc20TokenService>().
                As<IErc20TokenService>().SingleInstance();


            builder.RegisterType<ErcContractProcessor>().
                As<IErcContractProcessor>().SingleInstance();

            builder
                .RegisterType<AssetExtendedInfoService>()
                .As<IAssetExtendedInfoService>()
                .SingleInstance();

            builder
                .RegisterType<AssetService>()
                .As<IAssetService>()
                .SingleInstance();

            builder
                .RegisterType<AssetPairService>()
                .As<IAssetPairService>()
                .SingleInstance();

            builder
                .RegisterType<AssetGroupService>()
                .As<IAssetGroupService>()
                .SingleInstance();

            builder
                .RegisterType<AssetPairService>()
                .As<IAssetPairService>()
                .SingleInstance();

            builder
                .RegisterType<AssetSettingsService>()
                .As<IAssetSettingsService>()
                .SingleInstance();

            builder
                .RegisterType<IssuerService>()
                .As<IIssuerService>()
                .SingleInstance();

            builder
                .RegisterType<MarginAssetPairService>()
                .As<IMarginAssetPairService>()
                .SingleInstance();

            builder
                .RegisterType<MarginAssetService>()
                .As<IMarginAssetService>()
                .SingleInstance();

            builder
                .RegisterType<MarginIssuerService>()
                .As<IMarginIssuerService>()
                .SingleInstance();

            builder
                .RegisterType<WatchListService>()
                .As<IWatchListService>()
                .SingleInstance();
        }
    }
}