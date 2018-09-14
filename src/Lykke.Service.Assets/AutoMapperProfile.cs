using AutoMapper;
using Lykke.Service.Assets.Contract.Events;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Requests.v2.AssetConditions;
using Lykke.Service.Assets.Responses.v2.AssetConditions;
using Lykke.Service.Assets.Responses.V2;

namespace Lykke.Service.Assets
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IAsset, Asset>();
            CreateMap<IAssetAttributes, AssetAttributes>();
            CreateMap<IAssetAttribute, AssetAttribute>();
            CreateMap<IAssetCategory, AssetCategory>();
            CreateMap<IAssetDescription, AssetDescription>();
            CreateMap<IAssetExtendedInfo, AssetExtendedInfo>();
            CreateMap<IAssetGroup, AssetGroup>();
            CreateMap<IAssetPair, AssetPair>();
            CreateMap<IAssetSettings, AssetSettings>();
            CreateMap<IErc20Token, Erc20Token>();
            CreateMap<IIssuer, Issuer>();
            CreateMap<IMarginAsset, MarginAsset>();
            CreateMap<IMarginAssetPair, MarginAssetPair>();
            CreateMap<IMarginIssuer, MarginIssuer>();
            CreateMap<IWatchList, WatchList>();

            CreateMap<IAssetCondition, AssetConditionModel>(MemberList.Source);
            CreateMap<IAssetConditionLayer, AssetConditionLayerModel>(MemberList.Source)
                .ForMember(dest => dest.AssetConditions, opt => opt.MapFrom(src => src.AssetConditions))
                .ForMember(dest => dest.DefaultCondition, opt => opt.MapFrom(src => src.AssetDefaultCondition));

            CreateMap<IAssetDefaultCondition, AssetDefaultConditionModel>(MemberList.Source);
            CreateMap<IAssetDefaultConditionLayer, AssetDefaultConditionLayerModel>(MemberList.Source)
                .ForMember(dest => dest.AssetConditions, opt => opt.MapFrom(src => src.AssetConditions));

            CreateMap<EditAssetConditionModel, Services.Domain.AssetCondition>(MemberList.Destination);
            CreateMap<EditAssetConditionLayerModel, Services.Domain.AssetConditionLayer>(MemberList.Destination)
                .ForMember(dest => dest.AssetConditions, opt => opt.Ignore())
                .ForMember(dest => dest.AssetDefaultCondition, opt => opt.Ignore());

            CreateMap<EditAssetDefaultConditionModel, Services.Domain.AssetDefaultCondition>(MemberList.Destination);
            CreateMap<EditAssetDefaultConditionLayerModel, Services.Domain.AssetDefaultConditionLayer>(MemberList.Destination)
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.AssetConditions, opt => opt.Ignore());

            CreateMap<Services.Domain.Asset, AssetCreatedEvent>();
            CreateMap<Services.Domain.Asset, AssetUpdatedEvent>();
            CreateMap<Services.Domain.AssetPair, AssetPairCreatedEvent>();
            CreateMap<Services.Domain.AssetPair, AssetPairUpdatedEvent>();
        }
    }
}
