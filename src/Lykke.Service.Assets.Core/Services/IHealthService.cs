using System.Collections.Generic;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IHealthService
    {
        string GetHealthViolationMessage();

        IEnumerable<IHealthIssue> GetHealthIssues();
    }
}
