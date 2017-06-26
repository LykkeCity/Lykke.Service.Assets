using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetResponseModel
    {
        public string Id { get; set; }
        public string BlockChainId { get; set; }
        public string BlockChainAssetId { get; set; }
        public string Name { get; set; }
        public string Symbol { get; set; }
        public string IdIssuer { get; set; }
        [Required]
        public bool IsBase { get; set; }
        [Required]
        public bool HideIfZero { get; set; }
        [Required]
        public int Accuracy { get; set; }
        [Required]
        public double Multiplier { get; set; }
        [Required]
        public bool IsDisabled { get; set; }
        [Required]
        public bool HideWithdraw { get; set; }
        [Required]
        public bool HideDeposit { get; set; }
        [Required]
        public int DefaultOrder { get; set; }
        [Required]
        public bool KycNeeded { get; set; }
        public string AssetAddress { get; set; }
        [Required]
        public bool BankCardsDepositEnabled { get; set; }
        [Required]
        public bool SwiftDepositEnabled { get; set; }
        [Required]
        public bool BlockchainDepositEnabled { get; set; }
        [Required]
        public double DustLimit { get; set; }
        public string CategoryId { get; set; }


        public static AssetResponseModel Create(IAsset src)
        {
            return new AssetResponseModel
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