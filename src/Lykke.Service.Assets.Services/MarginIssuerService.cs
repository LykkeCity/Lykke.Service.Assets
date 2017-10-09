using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class MarginIssuerService : IMarginIssuerService
    {
        private readonly IMarginIssuerRepository _marginIssuerRepository;


        public MarginIssuerService(
            IMarginIssuerRepository marginIssuerRepository)
        {
            _marginIssuerRepository = marginIssuerRepository;
        }


        public async Task AddAsync(IMarginIssuer marginIssuer)
        {
            await _marginIssuerRepository.AddAsync(marginIssuer);
        }

        public IMarginIssuer CreateDefault()
        {
            return new MarginIssuer();
        }

        public async Task<IEnumerable<IMarginIssuer>> GetAllAsync()
        {
            return await _marginIssuerRepository.GetAllAsync();
        }

        public async Task<IMarginIssuer> GetAsync(string id)
        {
            return await _marginIssuerRepository.GetAsync(id);
        }

        public async Task UpdateAsync(IMarginIssuer marginIssuer)
        {
            await _marginIssuerRepository.UpdateAsync(marginIssuer);
        }
    }
}