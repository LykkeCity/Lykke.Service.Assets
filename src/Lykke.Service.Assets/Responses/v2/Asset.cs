using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class Asset : IAsset
    {
        public int Accuracy { get; set; }

        public string AssetAddress { get; set; }

        public bool BankCardsDepositEnabled { get; set; }

        public Blockchain Blockchain { get; set; }

        public string BlockChainAssetId { get; set; }

        public bool BlockchainDepositEnabled { get; set; }

        public string BlockChainId { get; set; }

        public bool BlockchainWithdrawal { get; set; }

        public bool BuyScreen { get; set; }

        public string CategoryId { get; set; }

        public bool CrosschainWithdrawal { get; set; }

        public int DefaultOrder { get; set; }

        public string DefinitionUrl { get; set; }

        public int? DisplayAccuracy { get; set; }

        public string DisplayId { get; set; }

        public double DustLimit { get; set; }

        public string ForwardBaseAsset { get; set; }

        public int ForwardFrozenDays { get; set; }

        public string ForwardMemoUrl { get; set; }

        public bool ForwardWithdrawal { get; set; }

        public bool HideDeposit { get; set; }

        public bool HideIfZero { get; set; }

        public bool HideWithdraw { get; set; }

        public string IconUrl { get; set; }

        public string Id { get; set; }

        public string IdIssuer { get; set; }

        public bool IsBase { get; set; }

        public bool IsDisabled { get; set; }

        public bool IsTradable { get; set; }

        public bool IssueAllowed { get; set; }

        public bool KycNeeded { get; set; }

        public double? LowVolumeAmount { get; set; }

        public int MultiplierPower { get; set; }

        public string Name { get; set; }

        public bool NotLykkeAsset { get; set; }

        public string[] PartnerIds { get; set; }

        public bool SellScreen { get; set; }

        public bool SwiftDepositEnabled { get; set; }

        public bool SwiftWithdrawal { get; set; }

        public string Symbol { get; set; }

        public AssetType? Type { get; set; }

        public bool IsTrusted { get; set; }
    }
}
