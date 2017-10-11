using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Models
{
    public class Issuer : IIssuer
    {
        public string IconUrl { get; set; }

        public string Id { get; set; }

        public string Name { get; set; }
    }
}