using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetSettingsEntity : EntityBase, IAssetSettings
    {
        public string Asset => RowKey;

        public decimal CashinCoef { get; set; }

        public string ChangeWallet { get; set; }

        public decimal Dust { get; set; }

        public string HotWallet { get; set; }

        public decimal? MaxBalance { get; set; }

        public int MaxOutputsCount { get; set; }

        public int MaxOutputsCountInTx { get; set; }

        public decimal MinBalance { get; set; }

        public int MinOutputsCount { get; set; }

        public decimal OutputSize { get; set; }

        public int PrivateIncrement { get; set; }
    }
}