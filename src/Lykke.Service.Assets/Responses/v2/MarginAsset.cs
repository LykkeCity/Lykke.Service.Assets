using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class MarginAsset : IMarginAsset
    {
        public int Accuracy { get; set; }

        public double DustLimit { get; set; }

        public string Id { get; set; }

        public string IdIssuer { get; set; }

        public double Multiplier { get; set; }

        public string Name { get; set; }

        public string Symbol { get; set; }
    }
}