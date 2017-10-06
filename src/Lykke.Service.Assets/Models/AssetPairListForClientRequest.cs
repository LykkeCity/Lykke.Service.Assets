namespace Lykke.Service.Assets.Models
{
    public class AssetPairListForClientRequest
    {
        public string ClientId { get; set; }

        public bool IsIosDevice { get; set; }

        public string PartnerId { get; set; }
    }
}