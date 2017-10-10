namespace Lykke.Service.Assets.Core.Domain
{
    public interface IMarginAsset
    {
        int Accuracy { get; }

        double DustLimit { get; }

        string Id { get; }

        string IdIssuer { get; }

        double Multiplier { get; }

        string Name { get; }

        string Symbol { get; }
    }
}