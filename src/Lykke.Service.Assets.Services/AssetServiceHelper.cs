﻿using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Repositories;

namespace Lykke.Service.Assets.Services
{
    public interface IAssetsServiceHelper
    {
        Task<IAsset[]> GetAssetsForClient(string clientId, bool isIosDevice, string partnerId = null);
    }

    /*public class AssetServiceHelper : IAssetsServiceHelper
    {
        private readonly IAssetGroupRepository _assetGroupRepo;
        private readonly IDictionaryManager<IAsset> _manager;

        public AssetServiceHelper(IDictionaryManager<IAsset> manager, IAssetGroupRepository assetGroupRepo)
        {
            _manager = manager;
            _assetGroupRepo = assetGroupRepo;
        }        

        public async Task<IAsset[]> GetAssetsForClient(string clientId, bool isIosDevice, string partnerId = null)
        {
            var result = (await _manager.GetAllAsync()).Where(x => !x.IsDisabled);

            if (partnerId != null)
            {
                return result.Where(x => x.PartnerIds != null && x.PartnerIds.Contains(partnerId)).ToArray();
            }

            var assetIdsForClient = await _assetGroupRepo.GetAssetIdsForClient(clientId, isIosDevice);

            if (assetIdsForClient != null)
                result = result.Where(x => assetIdsForClient.Contains(x.Id));

            return result.Where(x => !x.NotLykkeAsset).ToArray();
        }        
    }*/
}