using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
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
    }
}