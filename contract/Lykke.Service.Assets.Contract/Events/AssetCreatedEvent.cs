using MessagePack;

namespace Lykke.Service.Assets.Contract.Events
{

    [MessagePackObject(keyAsPropertyName: true)]
    public class AssetCreatedEvent
    {        
        public string Id { get; set; }

        public string Name { get; set; }        

        public string DisplayId { get; set; }        

        public int Accuracy { get; set; }        

        public double? LowVolumeAmount { get; set; }

        public double CashoutMinimalAmount { get; set; }        

        public string Symbol { get; set; }        

        public bool HideWithdraw { get; set; }        

        public bool HideDeposit { get; set; }        

        public bool KycNeeded { get; set; }        

        public bool BankCardsDepositEnabled { get; set; }        

        public bool SwiftDepositEnabled { get; set; }        

        public bool BlockchainDepositEnabled { get; set; }        

        public string CategoryId { get; set; }        

        public bool IsBase { get; set; }       

        public string IconUrl { get; set; }
    }
}
