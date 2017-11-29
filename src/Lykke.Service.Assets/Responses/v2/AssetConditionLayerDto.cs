using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetConditionLayerDto : IAssetConditionLayer
    {
        public AssetConditionLayerDto(string id, decimal priority, string description)
        {
            Id = id;
            Priority = priority;
            Description = description;
            AssetConditions = new List<AssetConditionDto>();
        }

        public AssetConditionLayerDto()
        {
            AssetConditions = new List<AssetConditionDto>();
        }

        public string Id { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public List<AssetConditionDto> AssetConditions { get; set; }

        IReadOnlyDictionary<string, IAssetConditions> IAssetConditionLayer.AssetConditions
            => AssetConditions.ToDictionary(e => e.Asset, e => (IAssetConditions)e);
    }
}
