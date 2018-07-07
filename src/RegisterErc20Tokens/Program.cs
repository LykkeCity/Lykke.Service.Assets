using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Lykke.Service.Assets.Client;
using Lykke.Service.Assets.Client.Models;
using Microsoft.Rest;
using Nethereum.Web3;
using Newtonsoft.Json;

namespace RegisterErc20Tokens
{
    internal class Program
    {
        private const string MetadataAbi = @"[{""constant"":true,""inputs"":[],""name"":""name"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""type"":""function""},{""constant"":true,""inputs"":[],""name"":""totalSupply"",""outputs"":[{""name"":""totalSupply"",""type"":""uint256""}],""payable"":false,""type"":""function""},{""constant"":true,""inputs"":[],""name"":""decimals"",""outputs"":[{""name"":"""",""type"":""uint8""}],""payable"":false,""type"":""function""},{""constant"":true,""inputs"":[],""name"":""symbol"",""outputs"":[{""name"":"""",""type"":""string""}],""payable"":false,""type"":""function""}]";

        private static readonly object OutputLock = new object();

        private static AssetsService _assetsService;
        private static int _outputCounter;
        private static Web3 _web3;


        private static void Main(string[] args)
        {
            if (args.Length < 3)
            {
                Console.WriteLine("You should provide assets service uri, web3 rpc endpoint uri and path to the list of contract addresses as arguments.");
            }

            if (!TryProcessArgs(args, out var processedArgs))
            {
                return;
            }

            _assetsService = new AssetsService(processedArgs.AssetsServiceUri);
            _web3 = new Web3(processedArgs.Web3Uri.ToString());

            var specification = GetSpecification(processedArgs.SpecificationPath);
            var contracts = specification.Tokens.ToArray();
            var groupsToAddTo = specification.Groups.ToArray();
            var skipTransferCheck = args.Length >= 4 && args[3] == "--skip-transfer-check";


            Console.WriteLine($"Registering {contracts.Length} tokens...");

            var assetIds = CreateAssets(contracts, skipTransferCheck).ToArray();

            if (groupsToAddTo.Any())
            {
                AddAssetsToGroups(assetIds, groupsToAddTo);
            }

            EnableAssets(assetIds);

            Console.ResetColor();
            Console.WriteLine("Operation completed. Press enter to exit.");
            Console.ReadLine();
        }

        private static void AddAssetsToGroups(IEnumerable<string> assetIds, IEnumerable<string> groups)
        {
            ResetOutputCounter();

            Console.ResetColor();
            Console.WriteLine("Adding assets to groups...");

            var assetsInGroups = groups.ToDictionary(x => x, x => _assetsService.AssetGroupGetAssetIds(x));

            Parallel.ForEach(assetIds, assetId =>
            {
                var operationResult = "";
                var operationSucceeded = true;

                try
                {
                    foreach (var groupToAssign in assetsInGroups.Keys)
                    {
                        if (!assetsInGroups[groupToAssign].Contains(assetId))
                        {
                            _assetsService.AssetGroupAddAsset(assetId, groupToAssign);
                        }
                    }

                    operationResult = "added to specified groups";
                }
                catch (Exception e)
                {
                    operationResult = e.ToOperationResult();
                    operationSucceeded = false;
                }

                OutputOperationResult(assetId, operationResult, operationSucceeded);
            });
        }

        private static bool ContractFiresTransferEvent(ContractMetadata contract)
        {
            var contractCode = _web3.Eth.GetCode.SendRequestAsync(contract.Address).Result;

            return contractCode.Contains("ddf252ad1be2c89b69c2b068fc378daa952ba7f163c4a11628f55a4df523b3ef");
        }

        private static bool ContractHasOrCallsTransferMethod(ContractMetadata contract)
        {
            var contractCode = _web3.Eth.GetCode.SendRequestAsync(contract.Address).Result;

            return contractCode.Contains("a9059cbb");
        }

        private static IEnumerable<string> CreateAssets(IEnumerable<ContractMetadata> contracts, bool skipTransferCheck)
        {
            var assetIds = new ConcurrentBag<string>();

            ResetOutputCounter();

            Console.WriteLine("Creating assets...");

            Parallel.ForEach(contracts, contract =>
            {
                var operationResult = "";
                var operationSucceeded = true;

                try
                {
                    var tokenAddress = contract.Address.ToLowerInvariant();
                    var erc20Token = _assetsService.Erc20TokenGetByAddress(tokenAddress);

                    if (erc20Token == null)
                    {
                        erc20Token = new Erc20Token
                        {
                            Address = tokenAddress
                        };

                        if (!skipTransferCheck)
                        {
                            if (!ContractHasOrCallsTransferMethod(contract))
                            {
                                operationResult = "transfer method not detected";
                                operationSucceeded = false;
                            }

                            if (!ContractFiresTransferEvent(contract))
                            {
                                operationResult = "contract never fires Transfer event";
                                operationSucceeded = false;
                            }
                        }

                        if (operationSucceeded)
                        {
                            if (TryGetTokenDecimals(contract, out var tokenDecimals))
                            {
                                erc20Token.TokenDecimals = tokenDecimals;
                            }
                            else
                            {
                                operationResult = "can not get token decimals";
                                operationSucceeded = false;
                            }
                        }

                        if (operationSucceeded)
                        {
                            if (TryGetTokenSymbol(contract, out var tokenSymbol))
                            {
                                erc20Token.TokenName = GetTokenName(contract, tokenSymbol);
                                erc20Token.TokenSymbol = tokenSymbol;
                            }
                            else
                            {
                                operationResult = "can not get token symbol";
                                operationSucceeded = false;
                            }
                        }

                        if (operationSucceeded)
                        {
                            erc20Token.TokenTotalSupply = GetTokenTotalSupply(contract);
                        }

                        // Bug workaround
                        try
                        {
                            _assetsService.Erc20TokenAdd(erc20Token);
                        }
                        catch (HttpOperationException e) when (e.Response.StatusCode == HttpStatusCode.OK)
                        {

                        }
                    }

                    if (operationSucceeded)
                    {
                        var assetId = erc20Token.AssetId ?? contract.AssetId;
                        Asset asset = null;
                        bool updateAsset = !string.IsNullOrEmpty(contract.AssetId);

                        if (updateAsset)
                        {
                            asset = _assetsService.AssetGet(contract.AssetId);

                            if (asset == null)
                            {
                                Console.WriteLine($"Wrong assetId {contract.AssetId}");

                                return;
                            }
                        }

                        if (!updateAsset && string.IsNullOrEmpty(assetId))
                        {
                            asset = _assetsService.Erc20TokenCreateAsset(contract.Address);

                            assetId = asset.Id;

                            operationResult = $"asset {assetId} created";
                        }
                        else
                        {
                            operationResult = "asset already exists";
                        }

                        if (updateAsset)
                        {
                            var oldTokens = _assetsService.Erc20TokenGetBySpecification(new Erc20TokenSpecification()
                            {
                                Ids = new List<string>()
                                    {
                                        assetId
                                    }
                            });

                            var oldToken = oldTokens.Items?.FirstOrDefault();
                            if (oldToken != null)
                            {
                                oldToken.AssetId = null;
                                _assetsService.Erc20TokenUpdate(oldToken);
                            }

                            asset.BlockChainAssetId = tokenAddress;
                            erc20Token = erc20Token ?? _assetsService.Erc20TokenGetByAddress(tokenAddress);
                            erc20Token.AssetId = assetId;
                            _assetsService.AssetUpdate(asset);
                            _assetsService.Erc20TokenUpdate(erc20Token);
                        }
                        else
                        {
                            assetIds.Add(assetId);
                        }
                    }
                }
                catch (Exception e)
                {
                    operationResult = e.ToOperationResult();
                    operationSucceeded = false;
                }

                OutputOperationResult(contract.Address, operationResult, operationSucceeded);
            });

            return assetIds;
        }

        private static void EnableAssets(IEnumerable<string> assetIds)
        {
            ResetOutputCounter();

            Console.ResetColor();
            Console.WriteLine("Enabling assets...");

            Parallel.ForEach(assetIds, assetId =>
            {
                var operationResult = "";
                var operationSucceeded = true;

                try
                {
                    _assetsService.AssetEnable(assetId);

                    operationResult = "asset enabled";
                }
                catch (Exception e)
                {
                    operationResult = e.ToOperationResult();
                    operationSucceeded = false;
                }

                OutputOperationResult(assetId, operationResult, operationSucceeded);
            });
        }

        private static bool TryProcessArgs(string[] args, out (Uri AssetsServiceUri, Uri Web3Uri, string SpecificationPath) processedArgs)
        {
            processedArgs = (null, null, null);

            try
            {
                processedArgs.AssetsServiceUri = new Uri(args[0], UriKind.Absolute);
            }
            catch (Exception)
            {
                Console.WriteLine("First argument should be a valid absolute uri string.");

                return false;
            }

            try
            {
                processedArgs.Web3Uri = new Uri(args[1], UriKind.Absolute);
            }
            catch (Exception)
            {
                Console.WriteLine("Second argument should be a valid absolute uri string.");

                return false;
            }

            try
            {
                if (!File.Exists(args[2]))
                {
                    throw new ArgumentException("Third argument should be a valid path to file.");
                }

                processedArgs.SpecificationPath = args[2];
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);

                return false;
            }

            return true;
        }


        private static Specification GetSpecification(string contractsPath)
        {
            var specification = File.ReadAllText(contractsPath);

            return JsonConvert.DeserializeObject<Specification>(specification);
        }

        private static string GetTokenName(ContractMetadata contract, string tokenSymbol)
        {
            return TryCallFunction<string>(contract, "name", out var name) ? name : tokenSymbol;
        }

        private static string GetTokenTotalSupply(ContractMetadata contract)
        {
            return TryCallFunction<BigInteger>(contract, "totalSupply", out var totalSupply) ? totalSupply.ToString() : null;
        }

        private static void OutputOperationResult(string id, string operationResult, bool operationSucceeded)
        {
            lock (OutputLock)
            {
                Console.ForegroundColor = operationSucceeded
                    ? ConsoleColor.Green
                    : ConsoleColor.Red;

                Console.WriteLine($"{++_outputCounter}. {id} - {operationResult}");
            }
        }

        private static void ResetOutputCounter()
        {
            _outputCounter = 0;
        }

        private static bool TryCallFunction<T>(ContractMetadata contract, string name, out T result)
        {
            result = default(T);

            try
            {
                result = _web3.Eth.GetContract(MetadataAbi, contract.Address).GetFunction(name).CallAsync<T>().Result;

                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private static bool TryGetTokenDecimals(ContractMetadata contract, out int tokenDecimals)
        {
            if (contract.Decimals.HasValue)
            {
                tokenDecimals = contract.Decimals.Value;

                return true;
            }

            if (TryCallFunction<int>(contract, "decimals", out var decimals))
            {
                tokenDecimals = decimals;

                return true;
            }

            tokenDecimals = 0;

            return false;
        }

        private static bool TryGetTokenSymbol(ContractMetadata contract, out string tokenSymbol)
        {
            if (!string.IsNullOrEmpty(contract.Symbol))
            {
                tokenSymbol = contract.Symbol;

                return true;
            }

            if (TryCallFunction<string>(contract, "symbol", out var symbol))
            {
                tokenSymbol = symbol;

                return true;
            }

            tokenSymbol = null;

            return false;
        }
    }
}
