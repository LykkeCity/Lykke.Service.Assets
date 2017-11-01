using Lykke.Service.Assets.Core.Domain;

namespace Lykke.Service.Assets.Responses.V2
{
    public class AssetCategory : IAssetCategory
    {
        public string AndroidIconUrl { get; set; }
        
        public string Id { get; set; }

        public string IosIconUrl { get; set; }
        
        public string Name { get; set; }
        
        public int SortOrder { get; set; }
    }
}
