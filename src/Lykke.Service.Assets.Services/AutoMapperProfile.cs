using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IAsset, Asset>();
            CreateMap<IErc20Token, Erc20Token>();
            CreateMap<IAssetCondition, AssetCondition>(MemberList.Source);
            CreateMap<IAssetConditionLayer, AssetConditionLayer>(MemberList.Source);
            CreateMap<IAssetConditionLayerSettings, AssetConditionLayerSettings>(MemberList.Source);
            CreateMap<IAssetDefaultCondition, AssetDefaultCondition>(MemberList.Source);
            CreateMap<IAssetDefaultConditionLayer, AssetDefaultConditionLayer>(MemberList.Source);
        }
    }
}
