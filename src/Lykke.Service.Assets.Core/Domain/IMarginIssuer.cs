namespace Lykke.Service.Assets.Core.Domain
{
    public interface IMarginIssuer
    {
        string IconUrl { get; set; }

        string Id { get; set; }

        string Name { get; set; }
    }
}