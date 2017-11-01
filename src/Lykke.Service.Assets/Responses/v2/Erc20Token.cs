﻿using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class Erc20Token : IErc20Token
    {
        public string AssetId { get; set; }
        
        public string Address { get; set; }

        public string BlockHash { get; set; }

        public int BlockTimestamp { get; set; }

        public string DeployerAddress { get; set; }

        public int? TokenDecimals { get; set; }

        public string TokenName { get; set; }

        public string TokenSymbol { get; set; }

        public string TokenTotalSupply { get; set; }

        public string TransactionHash { get; set; }
    }
}
