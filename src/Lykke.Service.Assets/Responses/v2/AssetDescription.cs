using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetDescription : IAssetDescription
    {
        public string AssetClass { get; set; }

        public string AssetDescriptionUrl { get; set; }

        public string AssetId { get; set; }

        public string Description { get; set; }

        public string FullName { get; set; }

        public string Id { get; set; }

        public string IssuerName { get; set; }

        public string MarketCapitalization { get; set; }

        public string NumberOfCoins { get; set; }

        [Required]
        public int PopIndex { get; set; }


        public static AssetDescription Create(IAssetDescription src)
        {
            return new AssetDescription
            {
                Id                   = src.Id,
                AssetId              = src.AssetId,
                AssetClass           = src.AssetClass,
                Description          = src.Description,
                IssuerName           = src.IssuerName,
                MarketCapitalization = src.MarketCapitalization,
                NumberOfCoins        = src.NumberOfCoins,
                PopIndex             = src.PopIndex,
                AssetDescriptionUrl  = src.AssetDescriptionUrl,
                FullName             = src.FullName
            };
        }
    }
}