using System;
using System.ComponentModel.DataAnnotations;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.v1
{
    [Obsolete]
    public class AssetResponseModel
    {
        // ReSharper disable UnusedAutoPropertyAccessor.Global
        // ReSharper disable MemberCanBePrivate.Global
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
        public int MultiplierPower { get; set; }
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
        public double DustLimit { get; set; }
        public string CategoryId { get; set; }
        [Required]
        public Blockchain Blockchain { get; set; }
        public string DefinitionUrl { get; set; }
        public string[] PartnerIds { get; set; }
        [Required]
        public bool NotLykkeAsset { get; set; }
        [Required]
        public bool IssueAllowed { get; set; }
        public double? LowVolumeAmount { get; set; }
        public string DisplayId { get; set; }

        [Required]
        public bool BankCardsDepositEnabled { get; set; }
        [Required]
        public bool SwiftDepositEnabled { get; set; }
        [Required]
        public bool BlockchainDepositEnabled { get; set; }
        [Required]
        public bool BuyScreen { get; set; }

        [Required]
        public bool SellScreen { get; set; }
        [Required]
        public bool BlockchainWithdrawal { get; set; }
        [Required]
        public bool SwiftWithdrawal { get; set; }
        [Required]
        public bool ForwardWithdrawal { get; set; }
        [Required]
        public bool CrosschainWithdrawal { get; set; }

        [Required]
        public int ForwardFrozenDays { get; set; }

        public string ForwardBaseAsset { get; set; }
        public string ForwardMemoUrl { get; set; }
        
        public string IconUrl { get; set; }
        // ReSharper restore UnusedAutoPropertyAccessor.Global
        // ReSharper restore MemberCanBePrivate.Global

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
                MultiplierPower = src.MultiplierPower,
                IsDisabled = src.IsDisabled,
                HideDeposit = src.HideDeposit,
                HideWithdraw = src.HideWithdraw,
                DefaultOrder = src.DefaultOrder,
                KycNeeded = src.KycNeeded,
                BankCardsDepositEnabled = src.BankCardsDepositEnabled,
                SwiftDepositEnabled = src.SwiftDepositEnabled,
                BlockchainDepositEnabled = src.BlockchainDepositEnabled,
                CategoryId = src.CategoryId,
                Blockchain = src.Blockchain,
                DefinitionUrl = src.DefinitionUrl,
                PartnerIds = src.PartnerIds,
                NotLykkeAsset = src.NotLykkeAsset,
                IssueAllowed = src.IssueAllowed,
                LowVolumeAmount = src.LowVolumeAmount,
                DisplayId = src.DisplayId,
                BuyScreen = src.BuyScreen,
                SellScreen = src.SellScreen,
                BlockchainWithdrawal = src.BlockchainWithdrawal,
                SwiftWithdrawal = src.SwiftWithdrawal,
                ForwardWithdrawal = src.ForwardWithdrawal,
                CrosschainWithdrawal = src.CrosschainWithdrawal,
                ForwardFrozenDays = src.ForwardFrozenDays,
                ForwardBaseAsset = src.ForwardBaseAsset,
                ForwardMemoUrl = src.ForwardMemoUrl,
                IconUrl = src.IconUrl,
                DustLimit = src.DustLimit
            };
        }
    }
}