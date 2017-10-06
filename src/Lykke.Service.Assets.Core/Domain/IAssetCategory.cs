namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetCategory
    {
        string AndroidIconUrl { get; set; }

        string Id { get; set; }

        string IosIconUrl { get; set; }

        string Name { get; set; }

        int SortOrder { get; set; }
    }
}