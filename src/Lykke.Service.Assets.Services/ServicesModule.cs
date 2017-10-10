﻿using Autofac;
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

            builder
                .RegisterType<AssetExtendedInfoService>()
                .As<IAssetExtendedInfoService>()
                .SingleInstance();

            builder
                .RegisterType<AssetService>()
                .As<IAssetService>()
                .SingleInstance();
        }
    }
}