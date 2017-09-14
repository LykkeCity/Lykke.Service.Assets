using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetCategory : IDictionaryItem
    {
        string Id { get; }
        string Name { get; }
        string IosIconUrl { get; set; }
        string AndroidIconUrl { get; set; }
        int SortOrder { get; set; }
    }

    public class AssetCategory : IAssetCategory
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string IosIconUrl { get; set; }
        public string AndroidIconUrl { get; set; }
        public int SortOrder { get; set; }

        public static AssetCategory Create(string id, string name, string iosIcon, string androidIcon, int sortOrder)
        {
            return new AssetCategory
            {
                Id = id,
                Name = name,
                IosIconUrl = iosIcon,
                AndroidIconUrl = androidIcon,
                SortOrder = sortOrder
            };
        }
    }

    public interface IAssetCategoryRepository
    {
        Task InsertAssetCategory(IAssetCategory assetCategory);

        Task DeleteAssetCategory(string assetCategoryId);

        Task UpdateAssetCategory(IAssetCategory assetCategory);
        Task<IAssetCategory> GetAssetCategory(string id);
    }
}
