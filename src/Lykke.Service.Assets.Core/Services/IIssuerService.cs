using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IIssuerService
    {
        Task<IIssuer> AddAsync(IIssuer issuer);
        
        Task<IEnumerable<IIssuer>> GetAllAsync();

        Task<IIssuer> GetAsync(string id);

        Task<IEnumerable<IIssuer>> GetAsync(string[] ids);

        Task RemoveAsync(string id);

        Task UpdateAsync(IIssuer issuer);
    }
}