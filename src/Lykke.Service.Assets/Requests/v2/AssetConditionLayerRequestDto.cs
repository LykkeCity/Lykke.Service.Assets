using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Requests.v2
{
    public class AssetConditionLayerRequestDto : IAssetConditionLayer
    {
        public AssetConditionLayerRequestDto(string id, decimal priority, string description,
            bool? clientsCanCashInViaBankCards, bool? swiftDepositEnabled)
        {
            Id = id;
            Priority = priority;
            Description = description;
            ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards;
            SwiftDepositEnabled = swiftDepositEnabled;
        }

        public AssetConditionLayerRequestDto()
        {
        }

        public string Id { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }

        IReadOnlyDictionary<string, IAssetConditions> IAssetConditionLayer.AssetConditions
            => new Dictionary<string, IAssetConditions>();
    }
}
