namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAsset
    {
        int Accuracy { get; }

        string AssetAddress { get; }

        bool BankCardsDepositEnabled { get; }

        bool OtherDepositOptionsEnabled { get; }

        Blockchain Blockchain { get; }

        string BlockChainAssetId { get; }

        bool BlockchainDepositEnabled { get; }

        string BlockChainId { get; }

        bool BlockchainWithdrawal { get; }

        string BlockchainIntegrationLayerId { get; set; }

        string BlockchainIntegrationLayerAssetId { get; set; }

        bool BuyScreen { get; }

        string CategoryId { get; }

        bool CrosschainWithdrawal { get; }

        int DefaultOrder { get; }

        string DefinitionUrl { get; }

        int? DisplayAccuracy { get; }

        string DisplayId { get; }

        double DustLimit { get; }

        /// <summary>
        ///     Base asset for forward withdrawal.
        /// </summary>
        string ForwardBaseAsset { get; }

        /// <summary>
        ///     Lock period for forward withdrawal.
        /// </summary>
        int ForwardFrozenDays { get; }

        string ForwardMemoUrl { get; }

        bool ForwardWithdrawal { get; }

        bool HideDeposit { get; }

        bool HideIfZero { get; }

        bool HideWithdraw { get; }

        string IconUrl { get; }

        string Id { get; }

        string IdIssuer { get; }

        bool IsBase { get; }

        bool IsDisabled { get; }

        bool IsTradable { get; }

        bool IssueAllowed { get; }

        bool KycNeeded { get; }

        /// <summary>
        ///     Value lower that this property is considered "low volume" and may have some limitations,
        ///     e.g. cash out timeout limits
        /// </summary>
        double? LowVolumeAmount { get; }

        int MultiplierPower { get; }

        string Name { get; }

        bool NotLykkeAsset { get; }

        string[] PartnerIds { get; }

        bool SellScreen { get; }

        bool SwiftDepositEnabled { get; }

        bool SwiftWithdrawal { get; }

        string Symbol { get; }

        AssetType? Type { get; }

        bool IsTrusted { get; }

        double CashinMinimalAmount { get; }

        double CashoutMinimalAmount { get; }
    }
}
