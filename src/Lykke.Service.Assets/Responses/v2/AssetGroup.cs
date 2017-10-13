using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetGroup : IAssetGroup
    {
        public bool ClientsCanCashInViaBankCards { get; set; }

        public bool IsIosDevice { get; set; }

        public string Name { get; set; }

        public bool SwiftDepositEnabled { get; set; }
    }
}