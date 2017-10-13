namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetSettings
    {
        string Asset { get; }

        decimal CashinCoef { get; }

        string ChangeWallet { get; }

        decimal Dust { get; }

        string HotWallet { get; }

        decimal? MaxBalance { get; }

        int MaxOutputsCount { get; }

        int MaxOutputsCountInTx { get; }

        decimal MinBalance { get; }

        int MinOutputsCount { get; }

        decimal OutputSize { get; }

        int PrivateIncrement { get; }
    }
}