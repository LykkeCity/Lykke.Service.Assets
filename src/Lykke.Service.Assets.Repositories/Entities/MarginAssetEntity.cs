using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class MarginAssetEntity : TableEntity, IMarginAsset
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