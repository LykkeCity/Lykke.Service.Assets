using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IMarginIssuerRepository
    {
        Task RegisterIssuerAsync(IMarginIssuer issuer);

        Task EditIssuerAsync(string id, IMarginIssuer issuer);

        Task<IEnumerable<IMarginIssuer>> GetIssuerAsync();

        Task<IMarginIssuer> GetIssuerAsync(string id);
    }
}