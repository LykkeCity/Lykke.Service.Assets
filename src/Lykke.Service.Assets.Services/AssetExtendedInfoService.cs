using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;


namespace Lykke.Service.Assets.Services
{
    public class AssetExtendedInfoService : IAssetExtendedInfoService
    {
        private readonly IAssetExtendedInfoRepository _assetExtendedInfoRepository;


        public AssetExtendedInfoService(
            IAssetExtendedInfoRepository assetExtendedInfoRepository)
        {
            _assetExtendedInfoRepository = assetExtendedInfoRepository;
        }


        public async Task<IAssetExtendedInfo> AddAsync(IAssetExtendedInfo assetInfo)
        {
            await _assetExtendedInfoRepository.UpsertAsync(assetInfo);

            return assetInfo;
        }

        public IAssetExtendedInfo CreateDefault(string id)
        {
            return new AssetExtendedInfo
            {
                Id                   = id,
                PopIndex             = 0,
                MarketCapitalization = string.Empty,
                Description          = string.Empty,
                AssetClass           = string.Empty,
                NumberOfCoins        = string.Empty,
                AssetDescriptionUrl  = string.Empty
            };
        }

        public async Task<IEnumerable<IAssetExtendedInfo>> GetAllAsync()
        {
            return await _assetExtendedInfoRepository.GetAllAsync();
        }

        public async Task<IAssetExtendedInfo> GetAsync(string id)
        {
            return await _assetExtendedInfoRepository.GetAsync(id);
        }

        public async Task RemoveAsync(string id)
        {
            await _assetExtendedInfoRepository.RemoveAsync(id);
        }

        public async Task UpdateAsync(IAssetExtendedInfo assetInfo)
        {
            await _assetExtendedInfoRepository.UpsertAsync(assetInfo);
        }
    }
}