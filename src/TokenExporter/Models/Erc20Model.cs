using System;
using System.Collections.Generic;
using System.Text;

namespace TokenExporter.Models
{
    public class Erc20Model
    {
        public string AssetId { get; set; }
        public string Address { get; set; }
        public string TokenName { get; set; }
        public string TokenSymbol { get; set; }
    }
}
