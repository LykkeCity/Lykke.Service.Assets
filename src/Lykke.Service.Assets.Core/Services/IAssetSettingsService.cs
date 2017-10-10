﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Core.Services
{
    public interface IAssetSettingsService
    {
        Task AddOrUpdateAsync(IAssetSettings settings);
        
        Task<IEnumerable<IAssetSettings>> GetAllAsync();

        Task<IAssetSettings> GetAsync(string asset);

        Task RemoveAsync(string asset);
    }
}