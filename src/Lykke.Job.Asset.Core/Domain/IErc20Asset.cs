using System;
using System.Collections.Generic;
using System.Text;

namespace Lykke.Job.Asset.Core.Domain
{
    public interface IErc20Asset
    {
        string AssetId { get; set; }

        string Address { get; set; }

        string BlockHash { get; set; }

        int BlockTimestamp { get; set; }

        string DeployerAddress { get; set; }

        uint? TokenDecimals { get; set; }

        string TokenName { get; set; }

        string TokenSymbol { get; set; }

        string TokenTotalSupply { get; set; }

        string TransactionHash { get; set; }
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
