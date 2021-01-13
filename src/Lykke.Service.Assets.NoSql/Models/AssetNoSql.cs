using System;
using Lykke.Service.Assets.Core.Domain;
using MyNoSqlServer.Abstractions;

namespace Lykke.Service.Assets.NoSql.Models
{
    public class AssetNoSql: MyNoSqlDbEntity
    {
        public const string TableName = "antares.asset.assets";

        public const string DefaultAssetPartitioKey = "--default--";
        public const string DefaultAssetRowKey = "--default--";

        public static string GeneratePartitionKey() => "assets";
        public static string GenerateRowKey(string assetId) => assetId;

        public AssetModel Asset { get; set; }

        public static AssetNoSql Create(IAsset source)
        {
            var item = new AssetNoSql()
            {
                PartitionKey = GeneratePartitionKey(),
                RowKey = GenerateRowKey(source.Id),
                Asset = AssetModel.Create(source)
            };
            return item;
        }

        public class AssetModel : IAsset
        {
            public static AssetModel Create(IAsset source)
            {
                var item = new AssetModel()
                {
                    Id = source.Id,
                    Name = source.Name,
                    Accuracy = source.Accuracy,
                    AssetAddress = source.AssetAddress,
                    BankCardsDepositEnabled = source.BankCardsDepositEnabled,
                    BlockChainAssetId = source.BlockChainAssetId,
                    BlockChainId = source.BlockChainId,
                    Blockchain = source.Blockchain,
                    BlockchainDepositEnabled = source.BlockchainDepositEnabled,
                    BlockchainIntegrationLayerAssetId = source.BlockchainIntegrationLayerAssetId,
                    BlockchainIntegrationLayerId = source.BlockchainIntegrationLayerId,
                    BlockchainIntegrationType = source.BlockchainIntegrationType,
                    BlockchainWithdrawal = source.BlockchainWithdrawal,
                    BuyScreen = source.BuyScreen,
                    CashinMinimalAmount = source.CashinMinimalAmount,
                    CashoutMinimalAmount = source.CashoutMinimalAmount,
                    CategoryId = source.CategoryId,
                    CrosschainWithdrawal = source.CrosschainWithdrawal,
                    DefaultOrder = source.DefaultOrder,
                    DefinitionUrl = source.DefinitionUrl,
                    DisplayAccuracy = source.DisplayAccuracy,
                    DisplayId = source.DisplayId,
                    DustLimit = source.DustLimit,
                    ForwardBaseAsset = source.ForwardBaseAsset,
                    ForwardFrozenDays = source.ForwardFrozenDays,
                    ForwardMemoUrl = source.ForwardMemoUrl,
                    ForwardWithdrawal = source.ForwardWithdrawal,
                    HideDeposit = source.HideDeposit,
                    HideIfZero = source.HideIfZero,
                    HideWithdraw = source.HideWithdraw,
                    IconUrl = source.IconUrl,
                    IdIssuer = source.IdIssuer,
                    IsBase = source.IsBase,
                    IsDisabled = source.IsDisabled,
                    IsTradable = source.IsTradable,
                    IsTrusted = source.IsTrusted,
                    IssueAllowed = source.IssueAllowed,
                    KycNeeded = source.KycNeeded,
                    LowVolumeAmount = source.LowVolumeAmount,
                    LykkeEntityId = source.LykkeEntityId,
                    MultiplierPower = source.MultiplierPower,
                    NotLykkeAsset = source.NotLykkeAsset,
                    OtherDepositOptionsEnabled = source.OtherDepositOptionsEnabled,
                    PartnerIds = source.PartnerIds,
                    PrivateWalletsEnabled = source.PrivateWalletsEnabled,
                    SellScreen = source.SellScreen,
                    SiriusAssetId = source.SiriusAssetId,
                    SiriusBlockchainId = source.SiriusBlockchainId,
                    SwiftDepositEnabled = source.SwiftDepositEnabled,
                    SwiftWithdrawal = source.SwiftWithdrawal,
                    Symbol = source.Symbol,
                    Type = source.Type
                };
                return item;
            }

            public int Accuracy { get; set; }
            public string AssetAddress { get; set; }
            public bool BankCardsDepositEnabled { get; set; }
            public bool OtherDepositOptionsEnabled { get; set; }
            public Blockchain Blockchain { get; set; }
            public string BlockChainAssetId { get; set; }
            public bool BlockchainDepositEnabled { get; set; }
            public string BlockChainId { get; set; }
            public bool BlockchainWithdrawal { get; set; }
            public string BlockchainIntegrationLayerId { get; set; }
            public string BlockchainIntegrationLayerAssetId { get; set; }
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
            public bool PrivateWalletsEnabled { get; set; }
            public double CashinMinimalAmount { get; set; }
            public double CashoutMinimalAmount { get; set; }

            /// <summary>
            /// The Lykke entity id, which provides trade venue for this asset.
            /// </summary>
            [Obsolete("No need reference to legal entity.")]
            public string LykkeEntityId { get; set; }

            public long SiriusAssetId { get; set; }
            public string SiriusBlockchainId { get; set; }
            public BlockchainIntegrationType BlockchainIntegrationType { get; set; }
        }
    }
}
