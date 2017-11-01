using System.Collections.Generic;
using System.Collections.Immutable;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class HealthService : IHealthService
    {
        public string GetHealthViolationMessage()
        {
            return null;
        }

        public IEnumerable<IHealthIssue> GetHealthIssues()
        {
            var issues = new List<IHealthIssue>();
            
            // Add health issues here, if necessary

            return issues.ToImmutableArray();
        }
    }
}
