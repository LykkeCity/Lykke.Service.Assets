using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetPair : IAssetPair
    {
        [Required]
        public int Accuracy { get; set; }

        public string BaseAssetId { get; set; }

        public string Id { get; set; }

        [Required]
        public int InvertedAccuracy { get; set; }

        [Required]
        public bool IsDisabled { get; set; }

        public string Name { get; set; }

        public string QuotingAssetId { get; set; }

        public string Source { get; set; }

        public string Source2 { get; set; }


        public static AssetPair Create(IAssetPair src)
        {
            return new AssetPair
            {
                Id               = src.Id,
                Name             = src.Name,
                BaseAssetId      = src.BaseAssetId,
                QuotingAssetId   = src.QuotingAssetId,
                Accuracy         = src.Accuracy,
                InvertedAccuracy = src.InvertedAccuracy,
                Source           = src.Source,
                Source2          = src.Source2,
                IsDisabled       = src.IsDisabled
            };
        }
    }
}