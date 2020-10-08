using Lykke.Service.Assets.Core.Domain;
using ProtoBuf;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    [ProtoContract]
    public class AssetDto : IAsset
    {
        [ProtoMember(1)]
        public int Accuracy { get; set; }

        [ProtoMember(2)]
        public string AssetAddress { get; set; }

        [ProtoMember(3)]
        public bool BankCardsDepositEnabled { get; set; }

        [ProtoMember(4)]
        public bool OtherDepositOptionsEnabled { get; set; }

        [ProtoMember(5)]
        public Blockchain Blockchain { get; set; }

        [ProtoMember(6)]
        public string BlockChainAssetId { get; set; }

        [ProtoMember(7)]
        public bool BlockchainDepositEnabled { get; set; }

        [ProtoMember(8)]
        public string BlockChainId { get; set; }

        [ProtoMember(9)]
        public bool BlockchainWithdrawal { get; set; }

        [ProtoMember(10)]
        public string BlockchainIntegrationLayerId { get; set; }

        [ProtoMember(11)]
        public string BlockchainIntegrationLayerAssetId { get; set; }

        [ProtoMember(12)]
        public bool BuyScreen { get; set; }

        [ProtoMember(13)]
        public string CategoryId { get; set; }

        [ProtoMember(14)]
        public bool CrosschainWithdrawal { get; set; }

        [ProtoMember(15)]
        public int DefaultOrder { get; set; }

        [ProtoMember(16)]
        public string DefinitionUrl { get; set; }

        [ProtoMember(17)]
        public int? DisplayAccuracy { get; set; }

        [ProtoMember(18)]
        public string DisplayId { get; set; }

        [ProtoMember(19)]
        public double DustLimit { get; set; }

        [ProtoMember(20)]
        public string ForwardBaseAsset { get; set; }

        [ProtoMember(21)]
        public int ForwardFrozenDays { get; set; }

        [ProtoMember(22)]
        public string ForwardMemoUrl { get; set; }

        [ProtoMember(23)]
        public bool ForwardWithdrawal { get; set; }

        [ProtoMember(24)]
        public bool HideDeposit { get; set; }

        [ProtoMember(25)]
        public bool HideIfZero { get; set; }

        [ProtoMember(26)]
        public bool HideWithdraw { get; set; }

        [ProtoMember(27)]
        public string IconUrl { get; set; }

        [ProtoMember(28)]
        public string Id { get; set; }

        [ProtoMember(29)]
        public string IdIssuer { get; set; }

        [ProtoMember(30)]
        public bool IsBase { get; set; }

        [ProtoMember(31)]
        public bool IsDisabled { get; set; }

        [ProtoMember(32)]
        public bool IsTradable { get; set; }

        [ProtoMember(33)]
        public bool IssueAllowed { get; set; }

        [ProtoMember(34)]
        public bool KycNeeded { get; set; }

        [ProtoMember(35)]
        public double? LowVolumeAmount { get; set; }

        [ProtoMember(36)]
        public int MultiplierPower { get; set; }

        [ProtoMember(37)]
        public string Name { get; set; }

        [ProtoMember(38)]
        public bool NotLykkeAsset { get; set; }

        [ProtoMember(39)]
        public string[] PartnerIds { get; set; }

        [ProtoMember(40)]
        public bool SellScreen { get; set; }

        [ProtoMember(41)]
        public bool SwiftDepositEnabled { get; set; }

        [ProtoMember(42)]
        public bool SwiftWithdrawal { get; set; }

        [ProtoMember(43)]
        public string Symbol { get; set; }

        [ProtoMember(44)]
        public AssetType? Type { get; set; }

        [ProtoMember(45)]
        public bool IsTrusted { get; set; }

        [ProtoMember(46)]
        public bool PrivateWalletsEnabled { get; set; }

        [ProtoMember(47)]
        public double CashinMinimalAmount { get; set; }

        [ProtoMember(48)]
        public double CashoutMinimalAmount { get; set; }

        [ProtoMember(49)]
        public string LykkeEntityId { get; set; }

        [ProtoMember(50)]
        public long SiriusAssetId { get; set; }
        [ProtoMember(51)]
        public string SiriusBlockchainId { get; set; }
        [ProtoMember(52)]
        public BlockchainIntegrationType BlockchainIntegrationType { get; set; }
    }
}
