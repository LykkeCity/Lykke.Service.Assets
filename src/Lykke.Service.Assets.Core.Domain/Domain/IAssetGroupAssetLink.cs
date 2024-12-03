namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetGroupAssetLink
    {
        string AssetId { get; }

        bool ClientsCanCashInViaBankCards { get; }

        bool IsIosDevice { get; }

        string GroupName { get; }

        bool SwiftDepositEnabled { get; }
    }
}