﻿using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetDefaultConditionDto : IAssetDefaultCondition
    {
        public bool? AvailableToClient { get; set; }
        public bool? IsTradable { get; set; }
        public bool? BankCardsDepositEnabled { get; set; }
        public bool? SwiftDepositEnabled { get; set; }
        public string Regulation { get; set; }
    }
}
