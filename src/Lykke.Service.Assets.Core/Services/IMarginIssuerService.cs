using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IMarginIssuerService
    {
        Task AddAsync(IMarginIssuer marginIssuer);

        IMarginIssuer CreateDefault();

        Task<IEnumerable<IMarginIssuer>> GetAllAsync();

        Task<IMarginIssuer> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(IMarginIssuer marginIssuer);
    }
}