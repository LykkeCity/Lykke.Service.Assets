namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetGroup
    {
        bool ClientsCanCashInViaBankCards { get; }
        
        bool IsIosDevice { get; }

        string Name { get; }

        bool SwiftDepositEnabled { get; }
    }
}