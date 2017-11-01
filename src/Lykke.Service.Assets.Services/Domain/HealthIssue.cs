using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Services.Domain
{
    public class HealthIssue : IHealthIssue
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
}
