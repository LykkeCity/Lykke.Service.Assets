using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class IssuerService : IIssuerService
    {
        private readonly IIssuerRepository _issuerRepository;

        public IssuerService(
            IIssuerRepository issuerRepository)
        {
            _issuerRepository = issuerRepository;
        }

        public async Task<IIssuer> AddAsync(IIssuer issuer)
        {
            await _issuerRepository.AddAsync(issuer);

            return issuer;
        }

        public async Task<IEnumerable<IIssuer>> GetAllAsync()
        {
            return await _issuerRepository.GetAllAsync();
        }

        public async Task<IIssuer> GetAsync(string id)
        {
            return await _issuerRepository.GetAsync(id);
        }

        public async Task<IEnumerable<IIssuer>> GetAsync(string[] ids)
        {
            return await _issuerRepository.GetAsync(ids);
        }

        public async Task RemoveAsync(string id)
        {
            await _issuerRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(IIssuer issuer)
        {
            await _issuerRepository.UpdateAsync(issuer);
        }
    }
}