using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IIssuerRepository
    {
        Task RegisterIssuerAsync(IIssuer issuer);

        Task EditIssuerAsync(string id, IIssuer issuer);

        Task<IEnumerable<IIssuer>> GetAllIssuersAsync();

        Task<IIssuer> GetIssuerAsync(string id);
    }
}