using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class AssetGroupClientLink : IAssetGroupClientLink
    {
        public bool ClientsCanCashInViaBankCards { get; set; }

        public string ClientId { get; set; }

        public string GroupName { get; set; }

        public bool IsIosDevice { get; set; }

        public bool SwiftDepositEnabled { get; set; }
    }
}