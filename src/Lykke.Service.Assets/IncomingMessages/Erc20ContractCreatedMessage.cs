namespace Lykke.Job.Asset.IncomingMessages
{
    // NOTE: This is incoming message example
    public class Erc20ContractCreatedMessage
    {
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