namespace Lykke.Service.Assets.Core.Domain
{
    public interface IAssetCategory
    {
        string AndroidIconUrl { get; }

        string Id { get; }

        string IosIconUrl { get; }

        string Name { get; }

        int SortOrder { get; }
    }
}