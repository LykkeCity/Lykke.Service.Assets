namespace Lykke.Service.Assets.Core.Domain
{
    public interface IMarginAsset
    {
        int Accuracy { get; set; }

        double DustLimit { get; set; }

        string Id { get; set; }

        string IdIssuer { get; set; }

        double Multiplier { get; set; }

        string Name { get; set; }

        string Symbol { get; set; }
    }
}