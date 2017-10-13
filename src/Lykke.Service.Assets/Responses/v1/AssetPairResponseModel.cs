using System;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.v1
{
    [Obsolete]
    public class AssetPairResponseModel
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string BaseAssetId { get; set; }
        public string QuotingAssetId { get; set; }
        [Required]
        public int Accuracy { get; set; }
        [Required]
        public int InvertedAccuracy { get; set; }
        public string Source { get; set; }
        public string Source2 { get; set; }
        [Required]
        public bool IsDisabled { get; set; }

        public static AssetPairResponseModel Create(IAssetPair src)
        {
            return new AssetPairResponseModel
            {
                Id = src.Id,
                Name = src.Name,
                BaseAssetId = src.BaseAssetId,
                QuotingAssetId = src.QuotingAssetId,
                Accuracy = src.Accuracy,
                InvertedAccuracy = src.InvertedAccuracy,
                Source = src.Source,
                Source2 = src.Source2,
                IsDisabled = src.IsDisabled
            };
        }
    }

    public class GetAssetPairsForClientRequestModel
    {
        public string ClientId { get; set; }
        public bool IsIosDevice { get; set; }
        public string PartnerId { get; set; }
    }
}