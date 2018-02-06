using System;
using System.Linq;
using System.Numerics;
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
            var erc20Token = Mapper.Map<Erc20Token>(await _erc20TokenService.GetByTokenAddressAsync(tokenAddress));

            if (!string.IsNullOrEmpty(erc20Token.AssetId))
            {
                throw new InvalidOperationException($"Asset for the token with specified address [{tokenAddress}] already exists.");
            }

            if (!string.IsNullOrEmpty(erc20Token.TokenTotalSupply) && !BigInteger.TryParse(erc20Token.TokenTotalSupply, out var _))
            {
                throw new InvalidOperationException($"Specified token supply [{erc20Token.TokenTotalSupply}] is not valid BigInteger.");
            }
            
            // Creating asset
            var asset                   = Mapper.Map<Asset>(_assetService.CreateDefault());
            var blockchainAndDisplayIds = await GetBlockchainAndDisplayIdsAsync(erc20Token);
            var tokenDecimals           = erc20Token.TokenDecimals ?? 0;

            asset.Accuracy          = GetAssetAccuracy(tokenDecimals);
            asset.AssetAddress      = string.Empty;
            asset.Blockchain        = Blockchain.Ethereum;
            asset.BlockChainId      = blockchainAndDisplayIds.BlockchainId;
            asset.BlockChainAssetId = erc20Token.Address;
            asset.DisplayId         = blockchainAndDisplayIds.DisplayId;
            asset.Id                = Guid.NewGuid().ToString();
            asset.IsDisabled        = true;
            asset.IsTradable        = false;
            asset.MultiplierPower   = tokenDecimals;
            asset.Name              = GetAssetName(erc20Token);
            asset.Symbol            = erc20Token.TokenSymbol;
            asset.Type              = AssetType.Erc20Token;

            asset = Mapper.Map<Asset>(await _assetService.AddAsync(asset));

            // Creating extended info
            var assetExtendedInfo = Mapper.Map<AssetExtendedInfo>(_assetExtendedInfoService.CreateDefault());

            assetExtendedInfo.Id            = asset.Id;
            assetExtendedInfo.NumberOfCoins = GetNumberOfCoins(erc20Token);
            

            await _assetExtendedInfoService.AddAsync(assetExtendedInfo);

            // Updating token
            erc20Token.AssetId = asset.Id;

            await _erc20TokenService.UpdateAsync(erc20Token);

            
            return asset;
        }

        private static int GetAssetAccuracy(int tokenDecimals)
        {
            const int defaultAccuracy = 6;

            return tokenDecimals >= defaultAccuracy ? defaultAccuracy : tokenDecimals;
        }

        private static string GetAssetName(IErc20Token token)
        {
            return !string.IsNullOrEmpty(token.TokenName)
                ? $"Erc20 token {token.TokenName} ({token.Address})"
                : $"Erc20 token {token.Address}";
        }

        private async Task<(string BlockchainId, string DisplayId)> GetBlockchainAndDisplayIdsAsync(IErc20Token token)
        {
            var assets        = (await _assetService.GetAllAsync(true)).ToArray();
            var blockchainIds = assets.Select(x => x.BlockChainId).ToArray();
            var displayIds    = assets.Select(x => x.DisplayId).ToArray();

            string GetUniqueId(string[] existingIds)
            {
                var uniqueId        = token.TokenSymbol;
                var duplicatesCount = 0;

                while (existingIds.Contains(uniqueId))
                {
                    duplicatesCount++;

                    uniqueId = $"{token.TokenSymbol}_{duplicatesCount + 1}";
                }

                return uniqueId;
            }

            return (GetUniqueId(blockchainIds), GetUniqueId(displayIds));
        }

        /// <summary>
        ///    Converts total supply from ethereum format to Lykke's number of coins
        /// </summary>
        /// <param name="token">
        ///    Instance of Erc20Token.
        /// </param>
        /// <returns>
        ///    String instance, that repesents total supply in Lykke's format.
        /// </returns>
        private static string GetNumberOfCoins(Erc20Token token)
        {
            if (string.IsNullOrEmpty(token.TokenTotalSupply))
            {
                return string.Empty;
            }
            else
            {
                var tokenDecimals = token.TokenDecimals ?? 0;

                if (tokenDecimals == 0)
                {
                    return token.TokenTotalSupply;
                }

                string numberOfCoins;

                if (tokenDecimals >= token.TokenTotalSupply.Length)
                {
                    numberOfCoins = token.TokenTotalSupply.PadLeft(tokenDecimals + 1, '0');
                    numberOfCoins = numberOfCoins.Insert(numberOfCoins.Length - tokenDecimals, ".");
                }
                else
                {
                    numberOfCoins = token.TokenTotalSupply.Insert(token.TokenTotalSupply.Length - tokenDecimals, ".");
                }

                numberOfCoins = numberOfCoins.TrimEnd('0').TrimEnd('.');

                return numberOfCoins;
            }
        }
    }
}
