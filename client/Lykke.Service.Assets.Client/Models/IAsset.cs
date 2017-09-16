// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.Assets.Client.Models
{
    using Lykke.Service;
    using Lykke.Service.Assets;
    using Lykke.Service.Assets.Client;
    using Newtonsoft.Json;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public partial class IAsset
    {
        /// <summary>
        /// Initializes a new instance of the IAsset class.
        /// </summary>
        public IAsset()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IAsset class.
        /// </summary>
        /// <param name="blockchain">Possible values include: 'None',
        /// 'Bitcoin', 'Ethereum'</param>
        public IAsset(string blockChainId = default(string), string blockChainAssetId = default(string), string name = default(string), string symbol = default(string), string idIssuer = default(string), bool? isBase = default(bool?), bool? hideIfZero = default(bool?), int? accuracy = default(int?), int? multiplierPower = default(int?), bool? isDisabled = default(bool?), bool? hideWithdraw = default(bool?), bool? hideDeposit = default(bool?), int? defaultOrder = default(int?), bool? kycNeeded = default(bool?), string assetAddress = default(string), double? dustLimit = default(double?), string categoryId = default(string), Blockchain? blockchain = default(Blockchain?), string definitionUrl = default(string), IList<string> partnerIds = default(IList<string>), bool? notLykkeAsset = default(bool?), bool? issueAllowed = default(bool?), double? lowVolumeAmount = default(double?), string displayId = default(string), bool? bankCardsDepositEnabled = default(bool?), bool? swiftDepositEnabled = default(bool?), bool? blockchainDepositEnabled = default(bool?), bool? buyScreen = default(bool?), bool? sellScreen = default(bool?), bool? blockchainWithdrawal = default(bool?), bool? swiftWithdrawal = default(bool?), bool? forwardWithdrawal = default(bool?), bool? crosschainWithdrawal = default(bool?), int? forwardFrozenDays = default(int?), string forwardBaseAsset = default(string), string forwardMemoUrl = default(string), string iconUrl = default(string), string id = default(string))
        {
            BlockChainId = blockChainId;
            BlockChainAssetId = blockChainAssetId;
            Name = name;
            Symbol = symbol;
            IdIssuer = idIssuer;
            IsBase = isBase;
            HideIfZero = hideIfZero;
            Accuracy = accuracy;
            MultiplierPower = multiplierPower;
            IsDisabled = isDisabled;
            HideWithdraw = hideWithdraw;
            HideDeposit = hideDeposit;
            DefaultOrder = defaultOrder;
            KycNeeded = kycNeeded;
            AssetAddress = assetAddress;
            DustLimit = dustLimit;
            CategoryId = categoryId;
            Blockchain = blockchain;
            DefinitionUrl = definitionUrl;
            PartnerIds = partnerIds;
            NotLykkeAsset = notLykkeAsset;
            IssueAllowed = issueAllowed;
            LowVolumeAmount = lowVolumeAmount;
            DisplayId = displayId;
            BankCardsDepositEnabled = bankCardsDepositEnabled;
            SwiftDepositEnabled = swiftDepositEnabled;
            BlockchainDepositEnabled = blockchainDepositEnabled;
            BuyScreen = buyScreen;
            SellScreen = sellScreen;
            BlockchainWithdrawal = blockchainWithdrawal;
            SwiftWithdrawal = swiftWithdrawal;
            ForwardWithdrawal = forwardWithdrawal;
            CrosschainWithdrawal = crosschainWithdrawal;
            ForwardFrozenDays = forwardFrozenDays;
            ForwardBaseAsset = forwardBaseAsset;
            ForwardMemoUrl = forwardMemoUrl;
            IconUrl = iconUrl;
            Id = id;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BlockChainId")]
        public string BlockChainId { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BlockChainAssetId")]
        public string BlockChainAssetId { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Name")]
        public string Name { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Symbol")]
        public string Symbol { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IdIssuer")]
        public string IdIssuer { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsBase")]
        public bool? IsBase { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "HideIfZero")]
        public bool? HideIfZero { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Accuracy")]
        public int? Accuracy { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "MultiplierPower")]
        public int? MultiplierPower { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IsDisabled")]
        public bool? IsDisabled { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "HideWithdraw")]
        public bool? HideWithdraw { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "HideDeposit")]
        public bool? HideDeposit { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DefaultOrder")]
        public int? DefaultOrder { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "KycNeeded")]
        public bool? KycNeeded { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "AssetAddress")]
        public string AssetAddress { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DustLimit")]
        public double? DustLimit { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CategoryId")]
        public string CategoryId { get; private set; }

        /// <summary>
        /// Gets possible values include: 'None', 'Bitcoin', 'Ethereum'
        /// </summary>
        [JsonProperty(PropertyName = "Blockchain")]
        public Blockchain? Blockchain { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DefinitionUrl")]
        public string DefinitionUrl { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "PartnerIds")]
        public IList<string> PartnerIds { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "NotLykkeAsset")]
        public bool? NotLykkeAsset { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IssueAllowed")]
        public bool? IssueAllowed { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "LowVolumeAmount")]
        public double? LowVolumeAmount { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "DisplayId")]
        public string DisplayId { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BankCardsDepositEnabled")]
        public bool? BankCardsDepositEnabled { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SwiftDepositEnabled")]
        public bool? SwiftDepositEnabled { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BlockchainDepositEnabled")]
        public bool? BlockchainDepositEnabled { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BuyScreen")]
        public bool? BuyScreen { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SellScreen")]
        public bool? SellScreen { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "BlockchainWithdrawal")]
        public bool? BlockchainWithdrawal { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "SwiftWithdrawal")]
        public bool? SwiftWithdrawal { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ForwardWithdrawal")]
        public bool? ForwardWithdrawal { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "CrosschainWithdrawal")]
        public bool? CrosschainWithdrawal { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ForwardFrozenDays")]
        public int? ForwardFrozenDays { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ForwardBaseAsset")]
        public string ForwardBaseAsset { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "ForwardMemoUrl")]
        public string ForwardMemoUrl { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "IconUrl")]
        public string IconUrl { get; private set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Id")]
        public string Id { get; private set; }

    }
}
