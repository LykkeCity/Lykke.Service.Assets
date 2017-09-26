using Lykke.Service.Assets.Core.Domain;
using System.Collections.Generic;
using System.Linq;

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
        int MultiplierPower { get; }
        bool IsDisabled { get; }
        bool HideWithdraw { get; }
        bool HideDeposit { get; }
        int DefaultOrder { get; }
        bool KycNeeded { get; }
        string AssetAddress { get; }
        double DustLimit { get; }
        string CategoryId { get; }
        Blockchain Blockchain { get; }
        string DefinitionUrl { get; }
        string[] PartnerIds { get; }
        bool NotLykkeAsset { get; }
        bool IssueAllowed { get; }
        /// <summary>
        /// Value lower that this property is considered "low volume" and may have some limitations,
        /// e.g. cash out timeout limits
        /// </summary>
        double? LowVolumeAmount { get; set; }
        string DisplayId { get; set; }

        //deposit flags
        bool BankCardsDepositEnabled { get; }
        bool SwiftDepositEnabled { get; }
        bool BlockchainDepositEnabled { get; }
        bool BuyScreen { get; }

        //withdraw flags
        bool SellScreen { get; }
        bool BlockchainWithdrawal { get; }
        bool SwiftWithdrawal { get; }
        bool ForwardWithdrawal { get; }
        bool CrosschainWithdrawal { get; }

        //lock period for forward withdrawal
        int ForwardFrozenDays { get; }
        //base asset for forward withdrawal
        string ForwardBaseAsset { get; }
        string ForwardMemoUrl { get; }

        string IconUrl { get; }
    }

    
}

public static class AssetsRepositoryExt
{
    public static string GetFirstAssetId(this IEnumerable<IAsset> assets)
    {
        return assets.OrderBy(x => x.DefaultOrder).First().Id;
    }

    public static IEnumerable<IAssetPair> WhichHaveAssets(this IEnumerable<IAssetPair> src, params string[] assetIds)
    {
        return src.Where(assetPair => assetIds.Contains(assetPair.BaseAssetId) || assetIds.Contains(assetPair.QuotingAssetId));
    }

    public static IEnumerable<IAssetPair> WhichConsistsOfAssets(this IEnumerable<IAssetPair> src, params string[] assetIds)
    {
        return src.Where(assetPair => assetIds.Contains(assetPair.BaseAssetId) && assetIds.Contains(assetPair.QuotingAssetId));
    }
}



