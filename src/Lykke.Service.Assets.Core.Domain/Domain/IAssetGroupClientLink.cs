namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetGroupClientLink
    {
        bool ClientsCanCashInViaBankCards { get; }

        string ClientId { get; }

        string GroupName { get; }

        bool IsIosDevice { get; }
        
        bool SwiftDepositEnabled { get; }
    }
}