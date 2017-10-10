using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class AssetSettingsService : IAssetSettingsService
    {
        private readonly IAssetSettingsRepository _assetSettingsRepository;


        public AssetSettingsService(
            IAssetSettingsRepository assetSettingsRepository)
        {
            _assetSettingsRepository = assetSettingsRepository;
        }

        public async Task AddAsync(IAssetSettings settings)
        {
            await _assetSettingsRepository.UpsertAsync(settings);
        }

        public async Task UpdateAsync(IAssetSettings settings)
        {
            await _assetSettingsRepository.UpsertAsync(settings);
        }

        public async Task<IEnumerable<IAssetSettings>> GetAllAsync()
        {
            return await _assetSettingsRepository.GetAllAsync();
        }

        public async Task<IAssetSettings> GetAsync(string asset)
        {
            return await _assetSettingsRepository.GetAsync(asset);
        }

        public async Task RemoveAsync(string asset)
        {
            await _assetSettingsRepository.RemoveAsync(asset);
        }
    }
}