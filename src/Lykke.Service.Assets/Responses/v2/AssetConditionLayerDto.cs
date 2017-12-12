using System.Collections.Generic;
using System.Linq;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetConditionLayerDto : IAssetConditionLayer
    {
        private AssetConditionLayerDto(string id, decimal priority, string description,
            bool? clientsCanCashInViaBankCards, bool? swiftDepositEnabled)
        {
            Id = id;
            Priority = priority;
            Description = description;
            ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards;
            SwiftDepositEnabled = swiftDepositEnabled;
            AssetConditions = new List<AssetConditionDto>();
        }

        public AssetConditionLayerDto(string id, decimal priority, string description,
            bool? clientsCanCashInViaBankCards, bool? swiftDepositEnabled,
            IReadOnlyDictionary<string, IAssetCondition> assetConditions)
            : this(id, priority, description, clientsCanCashInViaBankCards, swiftDepositEnabled)
        {
            AssetConditions = assetConditions.Values.Select(e => new AssetConditionDto(e.Asset, e.AvailableToClient, e.Regulation)).ToList();
        }

        public AssetConditionLayerDto()
        {
            AssetConditions = new List<AssetConditionDto>();
        }

        public string Id { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public List<AssetConditionDto> AssetConditions { get; set; }

        IReadOnlyDictionary<string, IAssetCondition> IAssetConditionLayer.AssetConditions
            => AssetConditions.ToDictionary(e => e.Asset, e => (IAssetCondition)e);
    }
}
