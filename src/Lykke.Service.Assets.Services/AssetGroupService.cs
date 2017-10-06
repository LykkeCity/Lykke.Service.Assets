using System.Linq;
using System.Threading.Tasks;
using Lykke.Service.Assets.Core.Repositories;
using Lykke.Service.Assets.Core.Services;

namespace Lykke.Service.Assets.Services
{
    public class AssetGroupService : IAssetGroupService
    {
        private readonly IAssetGroupRepository _assetGroupRepository;

        public AssetGroupService(
            IAssetGroupRepository assetGroupRepository)
        {
            _assetGroupRepository = assetGroupRepository;
        }


        public async Task<bool> CashInViaBankCardEnabled(string clientId, bool isIosDevice)
        {
            var assetGroups = (await _assetGroupRepository.GetAllAsync(AssetGroupType.GroupClientLink, clientId)).ToArray();

            if (assetGroups.Any(p => p.ClientsCanCashInViaBankCards && p.IsIosDevice == isIosDevice))
            {
                return true;
            }

            if (isIosDevice)
            {
                var clientIsNotAssignedToAnyIosGroup = !assetGroups.Any(x => x.IsIosDevice);
                if (clientIsNotAssignedToAnyIosGroup)
                {
                    return true;
                }
            }
            else
            {
                var clientIsNotAssignedToAnyNotIosGroup = assetGroups.Any(x => x.IsIosDevice);
                if (clientIsNotAssignedToAnyNotIosGroup)
                {
                    return true;
                }
            }

            return false;
        }

        public async Task<bool> SwiftDepositEnabled(string clientId, bool isIosDevice)
        {
            var assetGroups = (await _assetGroupRepository.GetAllAsync(AssetGroupType.GroupClientLink, clientId)).ToArray();

            if (assetGroups.Any(p => p.SwiftDepositEnabled && p.IsIosDevice == isIosDevice))
            {
                return true;
            }
            
            if (isIosDevice)
            {
                var clientIsNotAssignedToAnyIosGroup = !assetGroups.Any(x => x.IsIosDevice);
                if (clientIsNotAssignedToAnyIosGroup)
                {
                    return true;
                }
            }
            else
            {
                var clientIsNotAssignedToAnyNotIosGroup = assetGroups.Any(x => x.IsIosDevice);
                if (clientIsNotAssignedToAnyNotIosGroup)
                {
                    return true;
                }
            }

            return false;
        }
    }
}