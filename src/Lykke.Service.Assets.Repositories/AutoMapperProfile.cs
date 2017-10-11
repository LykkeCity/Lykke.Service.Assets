using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Repositories.DTOs;
using Lykke.Service.Assets.Repositories.Entities;
using Microsoft.WindowsAzure.Storage.Table;


namespace Lykke.Service.Assets.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // To entities

            CreateMap<IAsset, AssetEntity>()
                .ForMember(dest => dest.PartnersIdsJson, opt => opt.Ignore());

            CreateMap<IAssetGroup, AssetGroupEntity>()
                .ForMember(dest => dest.AssetId, opt => opt.Ignore())
                .ForMember(dest => dest.ClientId, opt => opt.Ignore());

            CreateMap<IAssetAttribute,    AssetAttributeEntity>();
            CreateMap<IAssetCategory,     AssetCategoryEntity>();
            CreateMap<IAssetExtendedInfo, AssetExtendedInfoEntity>();
            CreateMap<IAssetPair,         AssetPairEntity>();
            CreateMap<IAssetSettings,     AssetSettingsEntity>();
            CreateMap<IErc20Token,        Erc20TokenEntity>();
            CreateMap<IIssuer,            IssuerEntity>();
            CreateMap<IMarginAsset,       MarginAssetEntity>();
            CreateMap<IMarginAssetPair,   MarginAssetPairEntity>();
            CreateMap<IMarginIssuer,      MarginIssuerEntity>();
            CreateMap<IWatchList,         CustomWatchListEntity>();
            CreateMap<IWatchList,         PredefinedWatchListEntity>();
            
            ForAllMaps((map, cfg) =>
            {
                if (map.DestinationType.IsSubclassOf(typeof(TableEntity)))
                {
                    cfg.ForMember("ETag",         opt => opt.Ignore());
                    cfg.ForMember("PartitionKey", opt => opt.Ignore());
                    cfg.ForMember("RowKey",       opt => opt.Ignore());
                    cfg.ForMember("Timestamp",    opt => opt.Ignore());
                }
            });

            // From entities

            CreateMap<AssetGroupEntity, AssetGroupAssetLinkDto>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Name));

            CreateMap<AssetGroupEntity, AssetGroupClientLinkDto>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Name));

            CreateMap<AssetGroupEntity, ClientAssetGroupLinkDto>()
                .ForMember(dest => dest.GroupName, opt => opt.MapFrom(src => src.Name));

            CreateMap<AssetGroupEntity,          AssetGroupDto>();
            CreateMap<CustomWatchListEntity,     WatchListDto>();
            CreateMap<PredefinedWatchListEntity, WatchListDto>();
        }
    }
}