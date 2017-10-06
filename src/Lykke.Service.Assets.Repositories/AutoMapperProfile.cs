using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Repositories.Entities;

namespace Lykke.Service.Assets.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IAsset,         AssetEntity>();
            CreateMap<IAssetCategory, AssetCategoryEntity>();
            CreateMap<IAssetPair,     AssetPairEntity>();
            CreateMap<IIssuer,        IssuerEntity>();
        }
    }
}