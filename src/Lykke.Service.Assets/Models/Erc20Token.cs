using Lykke.Service.Assets.Core.Domain;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Lykke.Service.Assets.Models
{
    public class Erc20TokenModel : IErc20Asset
    {
        [Required]
        public string AssetId           { get; set; }
        [Required]
        public string Address           { get; set; }
        public string BlockHash         { get; set; }
        public int BlockTimestamp       { get; set; }
        public string DeployerAddress   { get; set; }
        public uint? TokenDecimals      { get; set; }
        public string TokenName         { get; set; }
        public string TokenSymbol       { get; set; }
        public string TokenTotalSupply  { get; set; }
        public string TransactionHash   { get; set; }
    } 
}
