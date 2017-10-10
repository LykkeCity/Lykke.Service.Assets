using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Models;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets
{
    internal class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IAssetAttributes, AssetAttributes>();
            CreateMap<IAssetAttribute,  AssetAttribute>();
            CreateMap<IAssetCategory,   AssetCategory>();
            CreateMap<IErc20Asset, Erc20TokenModel>().ReverseMap();

            #region Entities

            CreateMap<IErc20Asset, Erc20AssetEntity>().ReverseMap();

            #endregion
        }
    }
}
