﻿using Antares.Sdk.Services;
using Autofac;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services;

namespace Lykke.Service.Assets.Modules
{
    public class ServicesModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder
                .RegisterType<StartupManager>()
                .As<IStartupManager>()
                .SingleInstance();

            builder
                .RegisterType<AssetAttributeService>()
                .As<IAssetAttributeService>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            builder
                .RegisterType<AssetCategoryService>()
                .As<IAssetCategoryService>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            builder
                .RegisterType<Erc20TokenAssetService>()
                .As<IErc20TokenAssetService>()
                .SingleInstance();

            builder
                .RegisterType<Erc20TokenService>()
                .As<IErc20TokenService>()
                .SingleInstance();

            builder
                .RegisterType<AssetExtendedInfoService>()
                .As<IAssetExtendedInfoService>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            builder
                .RegisterType<AssetService>()
                .As<IAssetService>()
                .As<IStartable>()
                .AutoActivate()
                .SingleInstance();

            builder
                .RegisterType<AssetGroupService>()
                .As<IAssetGroupService>()
                .SingleInstance();

            builder
                .RegisterType<AssetConditionService>()
                .As<IAssetConditionService>()
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
