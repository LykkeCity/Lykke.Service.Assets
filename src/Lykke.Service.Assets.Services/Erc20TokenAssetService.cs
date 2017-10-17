using System;
using System.Threading.Tasks;
using AutoMapper;
using Lykke.Service.Assets.Core.Domain;
using Lykke.Service.Assets.Core.Services;
using Lykke.Service.Assets.Services.Domain;

namespace Lykke.Service.Assets.Services
{
    public class Erc20TokenAssetService : IErc20TokenAssetService
    {
        private readonly IAssetService             _assetService;
        private readonly IAssetExtendedInfoService _assetExtendedInfoService;
        private readonly IErc20TokenService        _erc20TokenService;

        public Erc20TokenAssetService(
            IAssetService assetService,
            IAssetExtendedInfoService assetExtendedInfoService,
            IErc20TokenService erc20TokenService)
        {
            _assetService             = assetService;
            _assetExtendedInfoService = assetExtendedInfoService;
            _erc20TokenService        = erc20TokenService;
        }

        public async Task<IAsset> CreateAssetForErc20TokenAsync(string tokenAddress)
        {
            var erc20Token        = Mapper.Map<Erc20Token>(await _erc20TokenService.GetByTokenAddressAsync(tokenAddress));
            var asset             = Mapper.Map<Asset>(_assetService.CreateDefault());
            var assetExtendedInfo = Mapper.Map<AssetExtendedInfo>(_assetExtendedInfoService.CreateDefault());
            

            asset.Blockchain = Blockchain.Ethereum;
            asset.Id         = Guid.NewGuid().ToString();
            asset.IsDisabled = true;
            asset.Name       = $"Asset for erc20 token {erc20Token.Address}";
            asset.Type       = AssetType.Erc20Token;

            asset = Mapper.Map<Asset>(await _assetService.AddAsync(asset));


            assetExtendedInfo.Id = asset.Id;

            await _assetExtendedInfoService.AddAsync(assetExtendedInfo);


            erc20Token.AssetId = asset.Id;

            await _erc20TokenService.UpdateAsync(erc20Token);


            return asset;
        }
    }
}