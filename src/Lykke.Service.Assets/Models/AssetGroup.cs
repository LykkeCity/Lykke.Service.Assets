using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class AssetGroup : IAssetGroup
    {
        public bool ClientsCanCashInViaBankCards { get; set; }

        public bool IsIosDevice { get; set; }

        public string Name { get; set; }

        public bool SwiftDepositEnabled { get; set; }
    }
}