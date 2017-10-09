using Lykke.Service.Asset.Core.Domain;
using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class Erc20AssetEntity : TableEntity, IErc20Asset
    {
        public string AssetId          { get; set; }
        public string Address          { get; set; }
        public string BlockHash        { get; set; }
        public int BlockTimestamp      { get; set; }
        public string DeployerAddress  { get; set; }
        public uint? TokenDecimals     { get; set; }
        public string TokenName        { get; set; }
        public string TokenSymbol      { get; set; }
        public string TokenTotalSupply { get; set; }
        public string TransactionHash  { get; set; }

        public static string GeneratePartitionKey()
        {
            return "Erc20Asset";
        }

        public static string GenerateRowKey(string address)
        {
            return address?.ToLower();
        }
    }
}