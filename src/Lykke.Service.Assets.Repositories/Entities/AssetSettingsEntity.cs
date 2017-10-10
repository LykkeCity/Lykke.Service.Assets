using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetSettingsEntity : TableEntity, IAssetSettings
    {
        public string Asset { get; set; }

        public decimal CashinCoef { get; set; }

        public string ChangeWallet { get; set; }

        public decimal Dust { get; set; }

        public string HotWallet { get; set; }

        public int MaxOutputsCount { get; set; }

        public int MaxOutputsCountInTx { get; set; }

        public decimal MinBalance { get; set; }

        public int MinOutputsCount { get; set; }

        public decimal OutputSize { get; set; }

        public int PrivateIncrement { get; set; }
    }
}