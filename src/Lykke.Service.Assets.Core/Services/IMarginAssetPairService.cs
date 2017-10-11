using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IMarginAssetPairService
    {
        Task<IMarginAssetPair> AddAsync(IMarginAssetPair marginAssetPair);

        IMarginAssetPair CreateDefault();

        Task<IEnumerable<IMarginAssetPair>> GetAllAsync();

        Task<IMarginAssetPair> GetAsync(string id);

        Task RemoveAsync(string id);

        Task UpdateAsync(IMarginAssetPair marginAssetPair);
    }
}