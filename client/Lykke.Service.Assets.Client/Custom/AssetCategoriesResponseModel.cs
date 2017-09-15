using Lykke.Service.Assets.Client.Custom;

namespace Lykke.Service.Assets.Client.Models
{
    public partial class AssetCategoriesResponseModel : IDictionaryItemModel, IAssetCategory
    {
        public ErrorResponse errorResponse { get; set; }

        public static AssetCategoriesResponseModel Create(ErrorResponse error)
        {
            return new AssetCategoriesResponseModel
            {
                errorResponse = error
            };

        }
        public static AssetCategoriesResponseModel Create(IAssetCategory category)
        {
            return new AssetCategoriesResponseModel
            {
                 Id = category.Id,
                  AndroidIconUrl = category.AndroidIconUrl,
                   IosIconUrl = category.IosIconUrl,
                    Name = category.Name,
                     SortOrder = category.SortOrder                     
            };

        }
    }
}
