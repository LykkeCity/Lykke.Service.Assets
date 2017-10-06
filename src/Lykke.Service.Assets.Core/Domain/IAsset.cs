namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAsset
    {
        int Accuracy { get; set; }

        string AssetAddress { get; set; }

        bool BankCardsDepositEnabled { get; set; }

        Blockchain Blockchain { get; set; }

        string BlockChainAssetId { get; set; }

        bool BlockchainDepositEnabled { get; set; }

        string BlockChainId { get; set; }

        bool BlockchainWithdrawal { get; set; }

        bool BuyScreen { get; set; }

        string CategoryId { get; set; }

        bool CrosschainWithdrawal { get; set; }

        int DefaultOrder { get; set; }

        string DefinitionUrl { get; set; }

        string DisplayId { get; set; }

        double DustLimit { get; set; }

        /// <summary>
        ///     Base asset for forward withdrawal.
        /// </summary>
        string ForwardBaseAsset { get; set; }

        /// <summary>
        ///     Lock period for forward withdrawal.
        /// </summary>
        int ForwardFrozenDays { get; set; }

        string ForwardMemoUrl { get; set; }

        bool ForwardWithdrawal { get; set; }

        bool HideDeposit { get; set; }

        bool HideIfZero { get; set; }

        bool HideWithdraw { get; set; }

        string IconUrl { get; set; }

        string Id { get; set; }

        string IdIssuer { get; set; }

        bool IsBase { get; set; }

        bool IsDisabled { get; set; }

        bool IssueAllowed { get; set; }

        bool KycNeeded { get; set; }

        /// <summary>
        ///     Value lower that this property is considered "low volume" and may have some limitations,
        ///     e.g. cash out timeout limits
        /// </summary>
        double? LowVolumeAmount { get; set; }

        int MultiplierPower { get; set; }

        string Name { get; set; }

        bool NotLykkeAsset { get; set; }

        string[] PartnerIds { get; set; }

        bool SellScreen { get; set; }

        bool SwiftDepositEnabled { get; set; }

        bool SwiftWithdrawal { get; set; }

        string Symbol { get; set; }
    }
}