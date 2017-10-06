namespace Lykke.Service.Assets.Models
{
    public class ExtendedAsset
    {
        public Asset Asset { get; set; }

        public AssetAttributes Attributes { get; set; }

        public AssetCategory Category { get; set; }

        public AssetDescription Description { get; set; }


        public static ExtendedAsset Create(Asset asset, AssetDescription description, AssetCategory category, AssetAttributes attributes)
        {
            return new ExtendedAsset
            {
                Asset       = asset,
                Description = description,
                Category    = category,
                Attributes  = attributes
            };
        }
    }
}