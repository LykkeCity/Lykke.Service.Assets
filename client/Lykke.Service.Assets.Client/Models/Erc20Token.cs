using Lykke.Service.Assets.Client.Cache;

namespace Lykke.Service.Assets.Client.Models
{
    public partial class Erc20Token : ICacheItem
    {
        public string Id => $"{this.AssetId}:{this.Address}";
    }
}
