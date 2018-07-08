using Newtonsoft.Json;

namespace RegisterErc20Tokens
{
    public class ContractMetadata
    {
        [JsonProperty(PropertyName = "address")]
        public string Address { get; set; }

        [JsonProperty(PropertyName = "decimal")]
        public int? Decimals { get; set; }

        [JsonProperty(PropertyName = "symbol")]
        public string Symbol { get; set; }

        [JsonProperty(PropertyName = "assetId")]
        public string AssetId { get; set; }
    }
}