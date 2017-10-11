using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class MarginIssuer : IMarginIssuer
    {
        public string IconUrl { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}
