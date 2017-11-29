﻿using System.Collections.Generic;
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

        public AssetConditionLayerDto(string id, decimal priority, string description, 
            bool? clientsCanCashInViaBankCards, bool? swiftDepositEnabled)
        {
            Id = id;
            Priority = priority;
            Description = description;
            ClientsCanCashInViaBankCards = clientsCanCashInViaBankCards;
            SwiftDepositEnabled = swiftDepositEnabled;
            AssetConditions = new Dictionary<string, IAssetConditions>();
        }

        public string Id { get; set; }
        public Dictionary<string, IAssetConditions> AssetConditions { get; set; }
        public decimal Priority { get; set; }
        public string Description { get; set; }
        public bool? ClientsCanCashInViaBankCards { get; set; }
        public bool? SwiftDepositEnabled { get; set; }

        IReadOnlyDictionary<string, IAssetConditions> IAssetConditionLayer.AssetConditions 
            => AssetConditions.ToDictionary(e => e.Key, e => e.Value as IAssetConditions);
    }
}
