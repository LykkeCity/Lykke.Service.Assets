namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetGroupAssetLink
    {
        string AssetId { get; set; }

        bool ClientsCanCashInViaBankCards { get; set; }

        bool IsIosDevice { get; set; }

        string GroupName { get; set; }

        bool SwiftDepositEnabled { get; set; }
    }
}