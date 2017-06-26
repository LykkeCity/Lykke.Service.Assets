namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAsset : IDictionaryItem
    {
        string BlockChainId { get; }
        string BlockChainAssetId { get; }
        string Name { get; }
        string Symbol { get; }
        string IdIssuer { get; }
        bool IsBase { get; }
        bool HideIfZero { get; }
        int Accuracy { get; }
        double Multiplier { get; }
        bool IsDisabled { get; }
        bool HideWithdraw { get; }
        bool HideDeposit { get; }
        int DefaultOrder { get; }
        bool KycNeeded { get; }
        string AssetAddress { get; }
        bool BankCardsDepositEnabled { get; }
        bool SwiftDepositEnabled { get; }
        bool BlockchainDepositEnabled { get; }
        double DustLimit { get; }
        string CategoryId { get; }
    }
}