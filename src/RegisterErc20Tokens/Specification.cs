using System.Collections.Generic;

namespace RegisterErc20Tokens
{
    public class Specification
    {
        public IEnumerable<string>Groups { get; set; }

        public IEnumerable<ContractMetadata> Tokens { get; set; }
    }
}