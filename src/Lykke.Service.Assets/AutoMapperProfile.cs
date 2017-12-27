using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Responses.v2;
using Lykke.Service.Assets.Responses.V2;

namespace Lykke.Service.Assets
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IAsset,             Asset>();
            CreateMap<IAssetAttributes,   AssetAttributes>();
            CreateMap<IAssetAttribute,    AssetAttribute>();
            CreateMap<IAssetCategory,     AssetCategory>();
            CreateMap<IAssetDescription,  AssetDescription>();
            CreateMap<IAssetExtendedInfo, AssetExtendedInfo>();
            CreateMap<IAssetGroup,        AssetGroup>();
            CreateMap<IAssetPair,         AssetPair>();
            CreateMap<IAssetSettings,     AssetSettings>();
            CreateMap<IErc20Token,        Erc20Token>();
            CreateMap<IIssuer,            Issuer>();
            CreateMap<IMarginAsset,       MarginAsset>();
            CreateMap<IMarginAssetPair,   MarginAssetPair>();
            CreateMap<IMarginIssuer,      MarginIssuer>();
            CreateMap<IWatchList,         WatchList>();

            CreateMap<IAssetCondition, AssetConditionDto>(MemberList.Source)
                .ReverseMap();

            CreateMap<IAssetConditionLayer, AssetConditionLayerDto>(MemberList.Source)
                .ForMember(dest => dest.AssetConditions, opt => opt.MapFrom(src => src.AssetConditions));

            CreateMap<AssetConditionDto, Services.Domain.AssetCondition>(MemberList.Destination);
            CreateMap<AssetConditionLayerDto, Services.Domain.AssetConditionLayer>(MemberList.Destination)
                .ForMember(dest => dest.AssetConditions, opt => opt.MapFrom(src => src.AssetConditions));

            CreateMap<AssetConditionSettings, Services.Domain.AssetCondition>(MemberList.Destination)
                .ForMember(dest => dest.Asset, opt => opt.Ignore());

            CreateMap<IAssetCondition, AssetConditionSettings> (MemberList.Source)
                .ForSourceMember(dest => dest.Asset, opt => opt.Ignore());

            CreateMap<IAssetConditionSettings, AssetConditionSettings>(MemberList.Source);
            CreateMap<AssetConditionSettings, Services.Domain.AssetConditionSettings>(MemberList.Destination);

            CreateMap<IAssetConditionLayerSettings, AssetConditionLayerSettings>(MemberList.Source);
            CreateMap<AssetConditionLayerSettings, Services.Domain.AssetConditionLayerSettings>(MemberList.Destination);
        }
    }
}
