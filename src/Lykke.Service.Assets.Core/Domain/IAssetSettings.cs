namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetSettings
    {
        string Asset { get; set; }

        decimal CashinCoef { get; set; }

        string ChangeWallet { get; set; }

        decimal Dust { get; set; }

        string HotWallet { get; set; }

        int MaxOutputsCount { get; set; }

        int MaxOutputsCountInTx { get; set; }

        decimal MinBalance { get; set; }

        int MinOutputsCount { get; set; }

        decimal OutputSize { get; set; }

        int PrivateIncrement { get; set; }
    }
}