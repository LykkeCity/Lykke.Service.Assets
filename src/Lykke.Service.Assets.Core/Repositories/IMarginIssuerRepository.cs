using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Repositories
{
    public interface IMarginIssuerRepository
    {
        Task AddAsync(IMarginIssuer marginIssuer);

        Task<IEnumerable<IMarginIssuer>> GetAllAsync();

        Task<IMarginIssuer> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(IMarginIssuer marginIssuer);
    }
}