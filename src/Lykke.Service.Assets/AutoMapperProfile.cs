﻿using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Models;

namespace Lykke.Service.Assets
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IAssetAttributes, AssetAttributes>();
            CreateMap<IAssetAttribute,  AssetAttribute>();

            //CreateMap<IAsset,            Asset>();
            
            //CreateMap<IAssetCategory,    AssetCategory>();
            //CreateMap<IAssetDescription, AssetDescription>();
            //CreateMap<IAssetPair,        AssetPair>();
        }
    }
}
