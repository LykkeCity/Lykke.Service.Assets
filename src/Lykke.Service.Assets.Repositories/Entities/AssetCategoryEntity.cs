using Lykke.Service.Assets.Core.Domain;
using Microsoft.WindowsAzure.Storage.Table;

namespace Lykke.Service.Assets.Repositories.Entities
{
    public class AssetCategoryEntity : TableEntity, IAssetCategory
    {
        public string AndroidIconUrl { get; set; }

        public string Id
        {
            get => RowKey;
            set => RowKey = value;
        }

        public string IosIconUrl { get; set; }

        public string Name { get; set; }

        public int SortOrder { get; set; }
    }
}
