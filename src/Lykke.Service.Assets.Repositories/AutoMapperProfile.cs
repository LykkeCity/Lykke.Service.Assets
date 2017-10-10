using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Repositories.Entities;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<IErc20Asset,    Erc20AssetEntity>();
            //CreateMap<IAsset,         AssetEntity>();
            CreateMap<IAssetCategory, AssetCategoryEntity>();
            //CreateMap<IAssetPair,     AssetPairEntity>();
            //CreateMap<IIssuer,        IssuerEntity>();
            
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
        }
    }
}