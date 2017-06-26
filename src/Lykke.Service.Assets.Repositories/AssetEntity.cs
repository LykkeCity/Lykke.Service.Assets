using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories
{
    public class AssetEntity : TableEntity, IAsset
    {
        public static string GeneratePartitionKey()
        {
            return "Asset";
        }

        public static string GenerateRowKey(string id)
        {
            return id;
        }

        public string Id => RowKey;
        public string BlockChainId { get; set; }
        public string BlockChainAssetId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string IdIssuer { get; set; }
        public bool IsBase { get; set; }
        public bool HideIfZero { get; set; }
        public int Accuracy { get; set; }
        public double Multiplier { get; set; }
        public bool IsDisabled { get; set; }
        public bool HideWithdraw { get; set; }
        public bool HideDeposit { get; set; }
        public int DefaultOrder { get; set; }
        public bool KycNeeded { get; set; }
        public string AssetAddress { get; set; }
        public bool BankCardsDepositEnabled { get; set; }
        public bool SwiftDepositEnabled { get; set; }
        public bool BlockchainDepositEnabled { get; set; }
        public double DustLimit { get; set; }
        public string CategoryId { get; set; }

        public static AssetEntity Create(IAsset asset)
        {
            return new AssetEntity
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(asset.Id),
                BlockChainId = asset.BlockChainId,
                Name = asset.Name,
                IsBase = asset.IsBase,
                Symbol = asset.Symbol,
                IdIssuer = asset.IdIssuer,
                HideIfZero = asset.HideIfZero,
                BlockChainAssetId = asset.BlockChainAssetId,
                Accuracy = asset.Accuracy,
                Multiplier = asset.Multiplier,
                IsDisabled = asset.IsDisabled,
                HideDeposit = asset.HideDeposit,
                HideWithdraw = asset.HideWithdraw,
                DefaultOrder = asset.DefaultOrder,
                KycNeeded = asset.KycNeeded,
                AssetAddress = asset.AssetAddress,
                BankCardsDepositEnabled = asset.BankCardsDepositEnabled,
                SwiftDepositEnabled = asset.SwiftDepositEnabled,
                BlockchainDepositEnabled = asset.BlockchainDepositEnabled,
                DustLimit = asset.DustLimit,
                CategoryId = asset.CategoryId
            };
        }
    }
}