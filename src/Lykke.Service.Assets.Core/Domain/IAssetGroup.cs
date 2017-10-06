namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetGroup
    {
        bool ClientsCanCashInViaBankCards { get; set; }

        string Id { get; set; }

        bool IsIosDevice { get; set; }

        string Name { get; set; }

        bool SwiftDepositEnabled { get; set; }
    }
}