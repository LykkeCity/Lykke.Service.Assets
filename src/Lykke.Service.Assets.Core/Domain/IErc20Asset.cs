namespace Lykke.Service.Assets.Core.Domain
{
    public interface IErc20Asset
    {
        string AssetId { get; }

        string Address { get; }

        string BlockHash { get; }

        int BlockTimestamp { get; }

        string DeployerAddress { get; }

        uint? TokenDecimals { get; }

        string TokenName { get; }

        string TokenSymbol { get; }

        string TokenTotalSupply { get; }

        string TransactionHash { get; }
    }

    public class Erc20Asset : IErc20Asset
    {
        public string AssetId { get; set; }

        public string Address { get; set; }

        public string BlockHash { get; set; }

        public int BlockTimestamp { get; set; }

        public string DeployerAddress { get; set; }

        public uint? TokenDecimals { get; set; }

        public string TokenName { get; set; }

        public string TokenSymbol { get; set; }

        public string TokenTotalSupply { get; set; }

        public string TransactionHash { get; set; }
    }
}
