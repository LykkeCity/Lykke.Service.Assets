// Code generated by Microsoft (R) AutoRest Code Generator 1.2.2.0
// Changes may cause incorrect behavior and will be lost if the code is
// regenerated.

namespace Lykke.Service.Assets.Client.Models
{
    using Lykke.Service;
    using Lykke.Service.Assets;
    using Lykke.Service.Assets.Client;
    using Newtonsoft.Json;
    using System.Linq;

    public partial class IAssetAttributesKeyValue
    {
        /// <summary>
        /// Initializes a new instance of the IAssetAttributesKeyValue class.
        /// </summary>
        public IAssetAttributesKeyValue()
        {
          CustomInit();
        }

        /// <summary>
        /// Initializes a new instance of the IAssetAttributesKeyValue class.
        /// </summary>
        public IAssetAttributesKeyValue(string key = default(string), string value = default(string))
        {
            Key = key;
            Value = value;
            CustomInit();
        }

        /// <summary>
        /// An initialization method that performs custom operations like setting defaults
        /// </summary>
        partial void CustomInit();

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Key")]
        public string Key { get; set; }

        /// <summary>
        /// </summary>
        [JsonProperty(PropertyName = "Value")]
        public string Value { get; set; }

    }
}