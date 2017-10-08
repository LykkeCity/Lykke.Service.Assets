using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetGroupAssetLink : IAssetGroupAssetLink
    {
        public string AssetId { get; set; }

        public bool ClientsCanCashInViaBankCards { get; set; }

        public bool IsIosDevice { get; set; }

        public string GroupName { get; set; }

        public bool SwiftDepositEnabled { get; set; }
    }
}