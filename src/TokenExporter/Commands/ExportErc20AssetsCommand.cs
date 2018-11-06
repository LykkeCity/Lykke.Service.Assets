using System;
using System.IO;
using System.Linq;
using System.Numerics;
using System.Threading.Tasks;
using Autofac;
using Lykke.Common.Log;
using Lykke.Service.Assets.Client;
using TokenExporter.Helpers;
using TokenExporter.Models;

namespace TokenExporter.Commands
{
    public class ExportErc20AssetsCommand : ICommand
    {
        private readonly IConfigurationHelper _helper;
        private readonly string _settingsUrl;

        public ExportErc20AssetsCommand(IConfigurationHelper helper,
            string settingsUrl)
        {
            _helper = helper;
            _settingsUrl = settingsUrl;
        }

        public async Task<int> ExecuteAsync()
        {
            return await ExportErc20AssetsCommandAsync(_settingsUrl);
        }

        private async Task<int> ExportErc20AssetsCommandAsync(
            string assetServiceUrl)
        {
            #region RegisterDependencies

            var (resolver, consoleLogger) = _helper.GetResolver(assetServiceUrl);

            #endregion

            var assetsService = resolver.Resolve<IAssetsService>();

            consoleLogger.Info("Started exporting");
            var erc20Tokens = await assetsService.Erc20TokenGetAllWithAssetsAsync();
            var assets =  await assetsService.AssetGetAllAsync();

            if (assets == null || assets.Count == 0)
            {
                consoleLogger.Error("No response from assets service/No assets in response.");
                return 0;
            }

            var assetsDict = assets.ToDictionary(x => x.Id);

            if (erc20Tokens?.Items == null || erc20Tokens.Items.Count == 0)
            {
                consoleLogger.Error("No response from assets service/No erc20 tokens in response.");
                return 0;
            }

            using (var writer = new StreamWriter("erc20Tokens.csv"))
            using (var csvWriter = new CsvHelper.CsvWriter(writer))
            {
                csvWriter.WriteHeader<Erc20Model>();
                csvWriter.NextRecord();

                foreach (var erc20Token in erc20Tokens?.Items)
                {
                    if (!assetsDict.TryGetValue(erc20Token.AssetId, out var asset))
                    {
                        continue;
                    }

                    var record = new Erc20Model
                    {
                        Address = erc20Token.Address,
                        AssetId = erc20Token.AssetId,
                        TokenName = erc20Token.TokenName,
                        TokenSymbol = erc20Token.TokenSymbol
                    };
                    csvWriter.WriteRecord(record);
                    csvWriter.NextRecord();
                }
            }

            consoleLogger.Info("Exporting has been completed!");

            return 0;
        }
    }
}
