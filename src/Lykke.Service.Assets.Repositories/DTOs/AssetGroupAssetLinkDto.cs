using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.DTOs
{
    public class AssetGroupAssetLinkDto : IAssetGroupAssetLink
    {
        public string AssetId { get; set; }

        public bool ClientsCanCashInViaBankCards { get; set; }

        public bool IsIosDevice { get; set; }

        public string GroupName { get; set; }

        public bool SwiftDepositEnabled { get; set; }
    }
}