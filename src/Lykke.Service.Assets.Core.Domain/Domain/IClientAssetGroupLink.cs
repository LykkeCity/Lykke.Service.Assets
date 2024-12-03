namespace Lykke.Service.Assets.Core.Domain
{
    public interface IClientAssetGroupLink
    {
        bool ClientsCanCashInViaBankCards { get; }

        string ClientId { get; }

        string GroupName { get; }

        bool IsIosDevice { get; }

        bool SwiftDepositEnabled { get; }
    }
}