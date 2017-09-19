using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class AssetDescriptionsResponseModel
    {
        public string Id { get; set; }
        public string AssetId { get; set; }
        public string AssetClass { get; set; }
        public string Description { get; set; }
        public string IssuerName { get; set; }
        public string NumberOfCoins { get; set; }
        public string MarketCapitalization { get; set; }
        [Required]
        public int PopIndex { get; set; }
        public string AssetDescriptionUrl { get; set; }
        public string FullName { get; set; }

        public static AssetDescriptionsResponseModel Create(IAssetDescription src)
        {
            return new AssetDescriptionsResponseModel
            {
                Id = src.Id,
                AssetId = src.AssetId,
                AssetClass = src.AssetClass,
                Description = src.Description,
                IssuerName = src.IssuerName,
                MarketCapitalization = src.MarketCapitalization,
                NumberOfCoins = src.NumberOfCoins,
                PopIndex = src.PopIndex,
                AssetDescriptionUrl = src.AssetDescriptionUrl,
                FullName = src.FullName
            };
        }
    }

    public class GetAssetDescriptionsRequestModel
    {
        public string[] Ids { get; set; }
    }
}
