using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetConditionLayerDto : IAssetConditionLayer
    {
        public AssetConditionLayerDto()
        {
            AssetConditions = new Dictionary<string, IAssetConditions>();
        }

        public string Id { get; set; }
        public Dictionary<string, IAssetConditions> AssetConditions { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }

        IReadOnlyDictionary<string, IAssetConditions> IAssetConditionLayer.AssetConditions 
            => AssetConditions.ToDictionary(e => e.Key, e => e.Value as IAssetConditions);
    }
}
