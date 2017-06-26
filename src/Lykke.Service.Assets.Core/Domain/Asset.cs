namespace Lykke.Service.Assets.Core.Domain
{
    public class Asset : IAsset
    {
        public string Id { get; private set; }
        public string BlockChainId { get; private set; }
        public string BlockChainAssetId { get; private set; }
        public string Name { get; private set; }
        public string Symbol { get; private set; }
        public string IdIssuer { get; private set; }
        public bool IsBase { get; private set; }
        public bool HideIfZero { get; private set; }
        public int Accuracy { get; private set; }
        public double Multiplier { get; private set; }
        public bool IsDisabled { get; private set; }
        public bool HideWithdraw { get; private set; }
        public bool HideDeposit { get; private set; }
        public int DefaultOrder { get; private set; }
        public bool KycNeeded { get; private set; }
        public string AssetAddress { get; private set; }
        public bool BankCardsDepositEnabled { get; private set; }
        public bool SwiftDepositEnabled { get; private set; }
        public bool BlockchainDepositEnabled { get; private set; }
        public double DustLimit { get; private set; }
        public string CategoryId { get; private set; }
        
        public static Asset Create(IAsset src)
        {
            return new Asset
            {
                Id = src.Id,
                Name = src.Name,
                Symbol = src.Symbol,
                IdIssuer = src.IdIssuer,
                IsBase = src.IsBase,
                BlockChainId = src.BlockChainId,
                BlockChainAssetId = src.BlockChainAssetId,
                HideIfZero = src.HideIfZero,
                AssetAddress = src.AssetAddress,
                Accuracy = src.Accuracy,
                Multiplier = src.Multiplier,
                IsDisabled = src.IsDisabled,
                HideDeposit = src.HideDeposit,
                HideWithdraw = src.HideWithdraw,
                DefaultOrder = src.DefaultOrder,
                KycNeeded = src.KycNeeded,
                BankCardsDepositEnabled = src.BankCardsDepositEnabled,
                SwiftDepositEnabled = src.SwiftDepositEnabled,
                BlockchainDepositEnabled = src.BlockchainDepositEnabled,
                CategoryId = src.CategoryId,
                DustLimit = src.DustLimit
            };
        }
    }
}