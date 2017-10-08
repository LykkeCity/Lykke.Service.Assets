namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetGroupClientLink
    {
        bool ClientsCanCashInViaBankCards { get; set; }

        string ClientId { get; set; }

        string GroupName { get; set; }

        bool IsIosDevice { get; set; }
        
        bool SwiftDepositEnabled { get; set; }
    }
}