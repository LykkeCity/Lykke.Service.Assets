using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetGroupEntity : TableEntity, IAssetGroup
    {
        public string AssetId { get; set; }

        public string ClientId { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }

        public bool IsIosDevice { get; set; }

        public bool ClientsCanCashInViaBankCards { get; set; }

        public bool SwiftDepositEnabled { get; set; }
    }
}